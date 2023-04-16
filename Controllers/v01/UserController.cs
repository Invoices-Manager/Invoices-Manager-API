﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Invoices_Manager_API.Controllers.v01
{
    [ApiController]
    [Route("api/v01/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly DataBaseContext _db;
        private readonly IConfiguration _config;

        public UserController(ILogger<UserController> logger, DataBaseContext db, IConfiguration config)
        {
            _logger = logger;
            _db = db;
            _config = config;
        }

        [TypeFilter(typeof(AuthFilter))]
        [HttpGet("WhoAmI")]
        public async Task<IActionResult> WhoAmI()
        {
            // Get the bearer token from the header
            var bearerToken = HttpContext.Request.Headers["bearerToken"].ToString();
            var user = await UserCore.GetCurrentUser(_db, bearerToken);

            //check if the user exist
            if (user is null)
                return NotFound($"The user does not exist");

            //censor => id, password, salt
            user.Id = 0;
            user.Password = "********************************";
            user.Salt = "********************************";

            //return the user as ok response
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserModel newUser)
        {
            //check if the id is empty
            if (newUser.Id != 0)
                return BadRequest("You are not allowed to set the ID! You get one assigned");

            //check if salt is empty
            if (!string.IsNullOrEmpty(newUser.Salt))
                return BadRequest("You are not allowed to set the Salt! You get one assigned");

            //check if the user exist
            if (_db.User.Any(x => x.Username == newUser.Username))
                return BadRequest($"The user with the username '{newUser.Username}' already exist");

            //check if he has manipulated data
            if (newUser.Invoices.Count != 0 || newUser.BackUps.Count != 0)
                return BadRequest("You are not allowed to set the Invoices, BackUps or Notebook!");

            //get a salt for the new user
            var newSalt = Security.PasswordHasher.GetNewSalt();

            //create the user
            var user = new UserModel
            {
                Username = newUser.Username,
                Password = Security.PasswordHasher.HashPassword(newUser.Password, newSalt),
                Salt = newSalt,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email
            };

            //add the user to the database
            await _db.User.AddAsync(user);
            await _db.SaveChangesAsync();

            //return the token
            return Ok(user);
        }

        [TypeFilter(typeof(AuthFilter))]
        [HttpDelete]
        public async Task<IActionResult> Remove()
        {
            // Get the bearer token from the header
            var bearerToken = HttpContext.Request.Headers["bearerToken"].ToString();
            var user = await UserCore.GetCurrentUser(_db, bearerToken);

            //check if the user exist
            if (user is null)
                return NotFound($"The user does not exist");

            //clear all data from the user before deleting
            _db.Invoice.RemoveRange(user.Invoices);
            _db.BackUp.RemoveRange(user.BackUps);
            _db.Note.RemoveRange(user.Notebook);
            _db.Logins.RemoveRange(user.Logins);

            //remove the user from the database
            _db.User.Remove(user);
            await _db.SaveChangesAsync();

            //remove the user data
            await FileCore.DeleteUserFolder(user);

            //return the user as ok response
            return Ok(user);
        }

        [Route("Login")]
        [HttpGet]
        public async Task<IActionResult> Login([FromBody] LoginModel newLogin)
        {
            //check if token is empty
            if (!string.IsNullOrEmpty(newLogin.Token))
                return BadRequest("You are not allowed to set the Token! You get one assigned");

            //check if the id is empty
            if (newLogin.Id != 0)
                return BadRequest("You are not allowed to set the ID! You get one assigned");

            //check if the date is empty
            if (newLogin.CreationDate != DateTime.MinValue)
                return BadRequest("You are not allowed to set the LoginDate!");

            //get the user
            var user = await _db.User
                .Include(x => x.Logins)
                .FirstOrDefaultAsync(x => x.Username == newLogin.Username);

            //check if the user exist
            if (user is null)
                return NotFound($"The user with the username '{newLogin.Username}' does not exist");

            //check if the user is safety locked
            if (user.IsBlocked)
                return BadRequest($"The user with the username '{newLogin.Username}' is locked, because the password would be entered incorrectly three times in a row. Please contact the admin");

            //check if the password is correct
            if (!Security.PasswordHasher.VerifyPassword(newLogin.Password, user.Password))
            {
                //increase the incorrect login attempt counter
                user.IncorrectLoginAttempts++;

                //check if the user is blocked, if yes then delete all login tokens
                if (user.IncorrectLoginAttempts >= 3)
                    _db.Logins.RemoveRange(user.Logins);

                await _db.SaveChangesAsync();

                return BadRequest("The password is not correct");
            }

            //set the incorrect login attempt counter to 0
            user.IncorrectLoginAttempts = 0;

            //create the login
            LoginModel successfulLogin = LoginCore.LoginUser(newLogin, user, _config);

            //save the token
            user.Logins.Add(successfulLogin);
            await _db.SaveChangesAsync();

            //check for old tokens and delete them
            await LoginCore.DeleteOldTokens(_db, user);

            //return the token
            return Ok(successfulLogin);
        }

        [TypeFilter(typeof(AuthFilter))]
        [Route("Logout")]
        [HttpDelete]
        public async Task<IActionResult> Logout()
        {
            // Get the bearer token from the header
            var bearerToken = HttpContext.Request.Headers["bearerToken"].ToString();
            var user = await UserCore.GetCurrentUser(_db, bearerToken);

            //check if the user is correct
            if (user is null)
                return NotFound($"The user does not exist");

            //check if this loginModel exist
            var logout = _db.Logins.FirstOrDefault(x => x.Token == bearerToken);
            if (logout is null)
                return NotFound($"There is no login with the token '{bearerToken}'");

            //delete the login
            _db.Logins.Remove(logout);
            await _db.SaveChangesAsync();

            //return the token
            return Ok();
        }

        [TypeFilter(typeof(AuthFilter))]
        [Route("LogoutEverywhere")]
        [HttpDelete]
        public async Task<IActionResult> LogoutEverywhere()
        {
            // Get the bearer token from the header
            var bearerToken = HttpContext.Request.Headers["bearerToken"].ToString();
            var user = await UserCore.GetCurrentUser(_db, bearerToken);

            //check if the user is correct
            if (user is null)
                return NotFound($"The user does not exist");

            //delete the login
            _db.Logins.RemoveRange(user.Logins);
            await _db.SaveChangesAsync();

            //return the token
            return Ok();
        }
    }
}
