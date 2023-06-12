using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Invoices_Manager_API.Security
{
    public class JWT
    {
        public static string GetJWT(UserModel user, IConfiguration config)
        {
            //set the keys in strings
            string? symmetricSecurityKey = config["JwtKeys:SymmetricSecurityKey"];
            string? issuer = config["JwtKeys:Issuer"];
            string? audience = config["JwtKeys:Audience"];
            int? expiration = Convert.ToInt32(config["JwtKeys:Expiration"]);

            //check if the keys are set
            if (String.IsNullOrEmpty(symmetricSecurityKey) || String.IsNullOrEmpty(issuer) || String.IsNullOrEmpty(audience) || expiration == null || expiration == 0)
                throw new Exception("JWT keys are not set or invalid!");


            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricSecurityKey));
            var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var Claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("ExpiresOn", DateTime.Now.AddMinutes(Convert.ToDouble(expiration)).ToString("yyyy-MM-dd HH:mm:ss"))
            };

            var JwtToken = new JwtSecurityToken(
                issuer,
                audience,
                Claims,
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
                string expiry = token.Claims.First(c => c.Type == "ExpiresOn").Value;

                //check if the value is valid
                if (string.IsNullOrEmpty(expiry))
                    return true;

                //set the "expiredOn" timestamp
                DateTime dateTime = DateTime.Parse(expiry);

                //check if it is already expired
                if (dateTime < DateTime.Now)
                    return true;

                //if the expiry is lower than DateTime.Now
                return false;
            }
            catch
            {
                return true;
            }
        }
    }
}
