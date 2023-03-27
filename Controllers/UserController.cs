using Invoices_Manager_API.Core;
using Invoices_Manager_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Invoices_Manager_API.Controllers
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

       
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserModel newUser)
        {
            //check if the id is empty
            if (newUser.Id != 0)
                return BadRequest("You are not allowed to set the ID! You get one assigned");

            //check if salt is empty
            if (!String.IsNullOrEmpty(newUser.Salt))
                return BadRequest("You are not allowed to set the Salt! You get one assigned");

            //check if the user exist
            if (_db.User.Any(x => x.Username == newUser.Username))
                return BadRequest($"The user with the username '{newUser.Username}' already exist");

            //check if he has manipulated data
            if (newUser.Invoices.Count != 0 || newUser.BackUps.Count != 0 )
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


        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel newLogin)
        {
            //check if token is empty
            if (!String.IsNullOrEmpty(newLogin.Token))
                return BadRequest("You are not allowed to set the Token! You get one assigned");

            //check if the id is empty
            if (newLogin.Id != 0)
                return BadRequest("You are not allowed to set the ID! You get one assigned");

            //check if the date is empty
            if (newLogin.LoginDate != DateTime.MinValue)
                return BadRequest("You are not allowed to set the LoginDate!");

            //get the user
            var user = await _db.User.FirstOrDefaultAsync(x => x.Username == newLogin.Username);

            //check if the user exist
            if (user is null)
                return NotFound($"The user with the username '{newLogin.Username}' does not exist");

            //check if the password is correct
            if (!Security.PasswordHasher.VerifyPassword(newLogin.Password ,user.Password))
                return BadRequest("The password is not correct");

            //create the login
            LoginModel successfulLogin = LoginCore.LoginUser(newLogin, user, _config);

            //save the token
            user.Logins.Add(successfulLogin);
            await _db.Logins.AddAsync(successfulLogin);
            await _db.SaveChangesAsync();

            //return the token
            return Ok(successfulLogin);
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout(int userId, string token)
        {
            //check if there is a token
            if (String.IsNullOrEmpty(token))
                return BadRequest("You have to send the Token!");

            //check if there is a user id
            if (userId == 0)
                return BadRequest("You have to send the User ID!");

            //get the user
            var user = await _db.User.FirstOrDefaultAsync(x => x.Id == userId);

            //check if the user is correct
            if (user is null)
                return NotFound($"The user with the user ID '{userId}' does not exist");

            //check if this loginModel exist
            var logout = _db.Logins.FirstOrDefault(x => x.Token == token);
            if (logout is null)
                return NotFound($"There is no login with the token '{token}' for the ID '{userId}'");

            //delete the login
            _db.Logins.Remove(logout);
            await _db.SaveChangesAsync();

            //return the token
            return Ok(token);
        }



        //[HttpDelete]
        //[Route("Remove")]
        //public async Task<IActionResult> Remove([FromBody] RegisterModel register)
        //{
        //    //check if the user exist
        //    if (_db.User.Any(x => x.Email == register.Email))
        //        return BadRequest($"The user with the email '{register.Email}' already exist");

        //    //create the user
        //    var user = new UserModel
        //    {
        //        Email = register.Email,
        //        Password = BCrypt.Net.BCrypt.HashPassword(register.Password),
        //        Role = "User"
        //    };

        //    //add the user to the database
        //    await _db.User.AddAsync(user);
        //    await _db.SaveChangesAsync();

        //    //generate the token
        //    var token = GenerateToken(user);

        //    //return the token
        //    return Ok(token);
        //}

        //private string GenerateToken(UserModel user)
        //{
        //    //create the claims
        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name, user.Email),
        //        new Claim(ClaimTypes.Role, user.Role)
        //    };

        //    //create the key
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("This is the secret key"));

        //    //create the credentials
        //    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    //create the token
        //    var token = new JwtSecurityToken(
        //        issuer: "InvoicesManager",
        //        audience: "InvoicesManager",
        //        claims: claims,
        //        expires: DateTime.Now.AddMinutes(30),
        //        signingCredentials: credentials
        //    );

        //    //return the token
        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
    }
}
