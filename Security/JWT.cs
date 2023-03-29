﻿using Invoices_Manager_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Invoices_Manager_API.Security
{
    public class JWT
    {
        public static string GetJWT(UserModel user, DateTime timeStamp, IConfiguration config)
        {
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtKeys:SymmetricSecurityKey"]));
            var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var Claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Expiration, timeStamp.AddMinutes(Convert.ToDouble(config["JwtKeys:Expiration"])).ToString("yyyy/MM/dd HH:mm:ss")),
            };

            var JwtToken = new JwtSecurityToken(
                config["JwtKeys:Issuer"],
                config["JwtKeys:Audience"],
                Claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(config["JwtKeys:Expiration"])),
                signingCredentials: Credentials);

            return new JwtSecurityTokenHandler().WriteToken(JwtToken);
        }

        public static bool CheckIfExpired(string? jwt)
        {
            try
            {
                //create jwt token
                var token = new JwtSecurityToken(jwtEncodedString: jwt);

                //get the expiry from the token
                string expiry = token.Claims.First(c => c.Type == "expiry").Value;

                //check if the value is valid
                if (!string.IsNullOrEmpty(expiry))
                    return true;

                //set the "expiredOn" timestamp
                DateTimeOffset dateTimeOffSet = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(expiry));
                DateTime expiredOn = dateTimeOffSet.DateTime;

                //check if it is already expired
                if (expiredOn < DateTime.Now)
                    return true;

                //if the expiry is lower than DateTime.Now
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}