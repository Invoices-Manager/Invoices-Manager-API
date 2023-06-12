using Invoices_Manager_API.Security;

namespace Invoices_Manager_API.Core
{
    public class LoginCore
    {
        public static LoginModel LoginUser(LoginModel newLogin, UserModel user, IConfiguration config)
        {
            DateTime timeStamp = DateTime.Now;

            return new LoginModel()
            {
                UserId = user.Id,
                Username = newLogin.Username,
                Password = newLogin.Password,
                CreationDate = timeStamp,
                Token = JWT.GetJWT(user, config)
            };
        }

        public static Task DeleteOldTokens(DataBaseContext db, UserModel user)
        {
            //scan all tokens from the user and delete the old ones which are expired
            foreach (var token in user.Logins)
            {
                if (JWT.CheckIfExpired(token.Token))
                    db.Logins.Remove(token);
            }

            return db.SaveChangesAsync();
        }
    }
}
