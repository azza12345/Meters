using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Core.Logging;

namespace MetersMVC
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizePermission : Attribute, IAuthorizationFilter
    {
        private readonly string _requiredPermission;

        public AuthorizePermission(string requiredPermission)
        {
            _requiredPermission = requiredPermission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var session = context.HttpContext.Session;
            string token =  session.GetString("JwtToken");

            //if (string.IsNullOrEmpty(token.ToString()))
            //{               
            //    context.Result = new UnauthorizedResult();
            //    return;
            //}

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }


            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken;

            try
            {
                jwtToken = jwtHandler.ReadJwtToken(token);
            }
            catch
            {
                
                context.Result = new UnauthorizedResult();
                return;
            }

           
            var permissions = jwtToken.Claims
                .Where(c => c.Type.EndsWith("_View") || c.Type.EndsWith("_Create") || c.Type.EndsWith("_Delete") || c.Type.EndsWith("_Update") || c.Type.EndsWith("_Read"))
                .Select(c => c.Type)
                .ToList();

            if (!permissions.Contains(_requiredPermission))
            {
                
                context.Result = new ForbidResult();
            }

        }
    }
}






