using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Domain.Models.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequiresClaimAttribute : Attribute, IAuthorizationFilter, IExceptionFilter
    {
        private readonly string _claimName;
        private readonly string _claimValue;
        public RequiresClaimAttribute(string claimName, string claimValue)
        {
            _claimName = claimName.ToUpper();
            _claimValue = claimValue;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.HasClaim("ADMIN", "True") && !context.HttpContext.User.HasClaim(_claimName, _claimValue))
            {
                context.Result = new ForbidResult();
            }
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var response = new ValidatedJwtResponseModel()
            {
                Success = false,
                Message = exception.Message,
                Token = null,
                Claims = null
            };
            context.Result = new ObjectResult(response)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
            context.ExceptionHandled = true;
        }
    }
}
