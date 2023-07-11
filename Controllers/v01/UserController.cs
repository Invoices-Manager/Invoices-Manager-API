using Invoices_Manager_API.Security;

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
            //set the users traceId
            Guid traceId = Guid.NewGuid();

            //get the user
            UserModel? user;
            using (var _uc = new UserCore())
            {
                var bearerToken = HttpContext.Request.Headers["bearerToken"].ToString();
                user = await _uc.GetCurrentUser(_db, bearerToken);
                if (user is null)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "An error occured while getting the user, faulty bearer token"));
            }

            //check if the user exist
            if (user is null)
                return new NotFoundObjectResult(ResponseMgr.CreateResponse(404, traceId, "The user does not exist"));

            //return the user as ok response
            return new OkObjectResult(ResponseMgr.CreateResponse(200, traceId, "The user was found successfully",
                new Dictionary<string, object> {
                    { "userName", user.Username },
                    { "email", user.Email },
                    { "firstName", user.FirstName },
                    { "lastName", user.LastName }
                }));

        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserModel newUser)
        {
            //set the users traceId
            Guid traceId = Guid.NewGuid();

            //check if the id is empty
            if (newUser.Id != 0)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "You are not allowed to set the ID! You get one assigned"));

            //check if salt is empty
            if (!string.IsNullOrEmpty(newUser.Salt))
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "You are not allowed to set the Salt!"));

            //check if the user exist
            if (_db.User.Any(x => x.Username == newUser.Username))
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The user already exists", 
                    new Dictionary<string, object> { 
                        { "userName", newUser.Username } 
                    }));

            ////check if he has manipulated data
            //if (newUser.Invoices.Count != 0 || newUser.BackUpInfos.Count != 0)
            //    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "You are not allowed to set the Invoices or BackUpInfos!"));

            //get a salt for the new user
            var newSalt = Security.PasswordHasher.GetNewSalt();

            //create the user
            var user = new UserModel
            {
                Username = newUser.Username,
                Password = PasswordHasher.HashPassword(newUser.Password, newSalt),
                Salt = newSalt,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email
            };

            //add the user to the database
            await _db.User.AddAsync(user);
            await _db.SaveChangesAsync();

            _logger.LogInformation($"User {user.Username} was created");

            //return the user
            return new OkObjectResult(ResponseMgr.CreateResponse(200, traceId, "The user was created successfully", 
                new Dictionary<string, object> { 
                    { "user", user }
                }));
        }

        [TypeFilter(typeof(AuthFilter))]
        [HttpDelete]
        public async Task<IActionResult> Remove()
        {
            //set the users traceId
            Guid traceId = Guid.NewGuid();

            //get the user
            UserModel? user;
            using (var _uc = new UserCore())
            {
                var bearerToken = HttpContext.Request.Headers["bearerToken"].ToString();
                user = await _uc.GetCurrentUser(_db, bearerToken);
                if (user is null)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "An error occured while getting the user, faulty bearer token"));
            }

            //check if the user exist
            if (user is null)
                return new NotFoundObjectResult(ResponseMgr.CreateResponse(404, traceId, "The user does not exist"));

            //clear all data from the user before deleting
            _db.Invoice.RemoveRange(user.Invoices);
            //_db.BackUpInfo.RemoveRange(user.BackUpInfos);
            _db.Note.RemoveRange(user.Notebook);
            _db.Logins.RemoveRange(user.Logins);

            //remove the user from the database
            _db.User.Remove(user);
            await _db.SaveChangesAsync();

            //remove the user data
            await FileCore.DeleteUserFolder(user);

            _logger.LogInformation($"User {user.Username} was deleted");

            //return the user as ok response
            return new OkObjectResult(ResponseMgr.CreateResponse(200, traceId, "The user was deleted successfully",
                    new Dictionary<string, object> {
                        { "userName", user.Username },
                        { "email", user.Email },
                        { "firstName", user.FirstName },
                        { "lastName", user.LastName }
                    }));
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel newLogin)
        {
            //set the users traceId
            Guid traceId = Guid.NewGuid();
            
            //check if token is empty
            if (!string.IsNullOrEmpty(newLogin.Token))
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "You are not allowed to set the Token!"));

            //check if the id is empty
            if (newLogin.Id != 0)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "You are not allowed to set the ID!"));

            //check if the date is empty
            if (newLogin.CreationDate != DateTime.MinValue)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "You are not allowed to set the CreationDate!"));

            //get the user
            var user = await _db.User
                .Include(x => x.Logins)
                .FirstOrDefaultAsync(x => x.Username == newLogin.Username);

            //check if the user exist
            if (user is null)
                return new NotFoundObjectResult(ResponseMgr.CreateResponse(404, traceId, "The user does not exist"));

            //check if the user is safety locked
            if (user.IsBlocked)
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, $"The user with the username '{newLogin.Username}' is locked, because the password would be entered incorrectly three times in a row. Please contact the admin"));

            //check if the password is correct
            if (!Security.PasswordHasher.VerifyPassword(newLogin.Password, user.Password))
            {
                //increase the incorrect login attempt counter
                user.IncorrectLoginAttempts++;

                //check if the user is blocked, if yes then delete all login tokens
                if (user.IncorrectLoginAttempts >= 3)
                    _db.Logins.RemoveRange(user.Logins);

                await _db.SaveChangesAsync();

                //return the error
                return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "The password is incorrect"));
            }

            //set the incorrect login attempt counter to 0
            user.IncorrectLoginAttempts = 0;

            //create the login
            LoginModel successfulLogin = LoginCore.LoginUser(newLogin, user, _config);

            //add it to the user 
            user.Logins.Add(successfulLogin);

            //save the jwt for the response and hash the model for the db
            string jwt = successfulLogin.Token;
            successfulLogin.Token = Hasher.GetSHA512Hash(jwt);

            //save the token
            user.Logins.Add(successfulLogin);
            await _db.SaveChangesAsync();

            //check for old tokens and delete them
            await LoginCore.DeleteOldTokens(_db, user);

            //return the token
            return new OkObjectResult(ResponseMgr.CreateResponse(200, traceId, "The login was successful",
                new Dictionary<string, object>{
                    { "token", jwt },
                    { "creationDate", successfulLogin.CreationDate },
                    { "userName", successfulLogin.Username }
                }
            ));
        }

        [TypeFilter(typeof(AuthFilter))]
        [Route("Logout")]
        [HttpDelete]
        public async Task<IActionResult> Logout()
        {
            //set the users traceId
            Guid traceId = Guid.NewGuid();

            //get the user
            UserModel? user;
            string bearerToken;
            using (var _uc = new UserCore())
            {
                bearerToken = HttpContext.Request.Headers["bearerToken"].ToString();
                user = await _uc.GetCurrentUser(_db, bearerToken);
                if (user is null)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "An error occured while getting the user, faulty bearer token"));
            }

            //check if the user is correct
            if (user is null)
                return new NotFoundObjectResult(ResponseMgr.CreateResponse(404, traceId, "The user does not exist"));

            //check if this loginModel exist
            var logout = _db.Logins.FirstOrDefault(x => x.Token == Hasher.GetSHA512Hash(bearerToken));
            if (logout is null)
                return new NotFoundObjectResult(ResponseMgr.CreateResponse(404, traceId, "The token does not exist",
                    new Dictionary<string, object> {
                        { "bearerToken", bearerToken }
                    }));

            //delete the login
            _db.Logins.Remove(logout);
            await _db.SaveChangesAsync();

            //return the token
            return new OkObjectResult(ResponseMgr.CreateResponse(200, traceId, "The logout was successful", 
                new Dictionary<string, object>{
                    { "userName", user.Username },
                    { "email", user.Email },
                    { "bearerToken", bearerToken }
                }));
        }

        [TypeFilter(typeof(AuthFilter))]
        [Route("LogoutEverywhere")]
        [HttpDelete]
        public async Task<IActionResult> LogoutEverywhere()
        {
            //set the users traceId
            Guid traceId = Guid.NewGuid();

            //get the user
            UserModel? user;
            using (var _uc = new UserCore())
            {
                var bearerToken = HttpContext.Request.Headers["bearerToken"].ToString();
                user = await _uc.GetCurrentUser(_db, bearerToken);
                if (user is null)
                    return new BadRequestObjectResult(ResponseMgr.CreateResponse(400, traceId, "An error occured while getting the user, faulty bearer token"));
            }

            //check if the user is correct
            if (user is null)
                return new NotFoundObjectResult(ResponseMgr.CreateResponse(404, traceId, "The user does not exist"));

            //save the count of logins
            int loginCounts = user.Logins.Count;

            //delete the login
            _db.Logins.RemoveRange(user.Logins);
            await _db.SaveChangesAsync();

            //return that all logins are deleted
            return new OkObjectResult(ResponseMgr.CreateResponse(200, traceId, "All logins were deleted successfully", 
                new Dictionary<string, object> { 
                    { "loginCounts", loginCounts }, 
                    { "userName", user.Username }, 
                    { "email", user.Email } 
                }));
        }
    }
}
