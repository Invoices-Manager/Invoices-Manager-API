using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Invoices_Manager_API.Core
{
    public class UserCore : IDisposable
    {
        public async Task<UserModel?> GetCurrentUser(DataBaseContext _db, string bearerToken)
        {
            //get from the bearer token the username
            //create jwt token
            var token = new JwtSecurityToken(jwtEncodedString: bearerToken);

            //get the expiry from the token
            var userName = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            //get the whole user form the db
            var user = _db.User
                .FirstOrDefaultAsync(x => x.Username == userName);

            return await user;
        }

        public void Dispose()
            => GC.SuppressFinalize(this);
    }
}
