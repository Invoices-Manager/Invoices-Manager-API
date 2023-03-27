using Invoices_Manager_API.Models;

namespace Invoices_Manager_API.Core
{
    public class LoginCore
    {
        public static LoginModel LoginUser(LoginModel newLogin, UserModel user, IConfiguration config)
        {
            DateTime timeStamp = DateTime.Now;

            return new LoginModel()
            {
                Username = newLogin.Username,
                Password = newLogin.Password,
                LoginDate = timeStamp,
                Token = JWTCore.GetJWT(user, timeStamp, config)
            };
        }
    }
}
