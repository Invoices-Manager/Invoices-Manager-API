﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Invoices_Manager_API.Core
{
    public class UserCore : IDisposable
    {
        private readonly DataBaseContext _db;

        public UserCore(DataBaseContext db)
        {
            _db = db;
        }
        
        public async Task<UserModel?> GetCurrentUser(string bearerToken, GetUserTypeEnum getStatement = GetUserTypeEnum.User)
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

                    GetUserTypeEnum.User => await _db.User
                                                          .FirstOrDefaultAsync(x => x.Username == userName),

                    _ => await _db.User
                                       .FirstOrDefaultAsync(x => x.Username == userName),
                };

                return user;
            });
        }

        public void Dispose()
        {
            _db.ChangeTracker.Clear();
            GC.Collect();
        }
    }
}
