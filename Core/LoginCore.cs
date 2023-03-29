using Invoices_Manager_API.Models;
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
                LoginDate = timeStamp,
                Token = JWT.GetJWT(user, timeStamp, config)
            };
        }

        internal static LoginModel LogoutUser(LoginModel newLogout, UserModel user)
        {
            throw new NotImplementedException();
        }
    }
}
