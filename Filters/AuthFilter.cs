using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Invoices_Manager_API.Filters
{
    //allow where I can use this attribute
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthFilter : Attribute, IAsyncActionFilter
    {
        private readonly DataBaseContext _db;

        public AuthFilter(DataBaseContext db)
            => _db = db;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //check if the header has a bearerToken, if not there is no key
            if (!context.HttpContext.Request.Headers.TryGetValue("bearerToken", out var potentialBearerToken))
            {
                context.Result = new UnauthorizedObjectResult("You dont have any bearerToken in your header");
                return;
            }

            //get the bearerToken from the header
            var bearerToken = potentialBearerToken.FirstOrDefault();

            //check if this bearerToken is in the db
            if (_db.Logins.Any(x => x.Token == bearerToken))
            {
                context.Result = new UnauthorizedObjectResult("You bearerToken is not valid");
                return;
            }

            //check if this bearerToken has not expired
            if (Security.JWT.CheckIfExpired(bearerToken))
            {
                //return a 401 with a message that the token is expired
                context.Result = new UnauthorizedObjectResult("Your is bearerToken expired! Get a new one!");
                return;
            }

            //when the bearerToken is valid and not expired
            await next();
        }
    }
}
