using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Invoices_Manager_API.Core
{
    public class UserCore : IDisposable
    {
        public async Task<UserModel?> GetCurrentUser(DataBaseContext _db, string bearerToken, GetUserTypeEnum getStatement)
        {
            //get from the bearer token the username
            //create jwt token
            var token = new JwtSecurityToken(jwtEncodedString: bearerToken);

            //get the username from the token
            var userName = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            //get the user form the db
            return await Task.Run(async () =>
            {              
                var user = getStatement switch
                {
                    GetUserTypeEnum.All => await _db.User
                                                         .Include(x => x.Invoices)
                                                         .Include(x => x.Notebook)
                                                         .Include(x => x.Logins)
                                                         .FirstOrDefaultAsync(x => x.Username == userName),
                                                         
                    GetUserTypeEnum.Notes => await _db.User
                                                           .Include(x => x.Notebook)
                                                           .FirstOrDefaultAsync(x => x.Username == userName),
                                                           
                    GetUserTypeEnum.Invoices => await _db.User
                                                              .Include(x => x.Invoices)
                                                              .FirstOrDefaultAsync(x => x.Username == userName),
                                                              
                    GetUserTypeEnum.Logins => await _db.User
                                                            .Include(x => x.Logins)
                                                            .FirstOrDefaultAsync(x => x.Username == userName),
                                                            
                    _ => await _db.User
                                       .Include(x => x.Invoices)
                                       .Include(x => x.Notebook)
                                       .Include(x => x.Logins)
                                       .FirstOrDefaultAsync(x => x.Username == userName),
                };

                return user;
            });
        }

        public void Dispose()
            => GC.SuppressFinalize(this);
    }
}
