﻿using Invoices_Manager_API.Models;
using Invoices_Manager_API.Security;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Invoices_Manager_API.Core
{
    public class UserCore
    {
        public static UserModel? GetCurrentUser(DataBaseContext _db, string bearerToken)
        {
            //get from the bearer token the username
            //create jwt token
            var token = new JwtSecurityToken(jwtEncodedString: bearerToken);

            //get the expiry from the token
            var userName = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            //get the whole user form the db
            var user = _db.User
                .Include(x => x.Invoices)
                .Include(x => x.BackUps)
                .Include(x => x.Notebook)
                .Include(x => x.Logins)
                .FirstOrDefault(x => x.Username == userName); 

            return user;
        }
    }
}