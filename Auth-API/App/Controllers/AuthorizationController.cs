using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace App.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private IAuthService _service;
        public AuthorizationController(IAuthService service)
        {
            _service = service;
        }


        [HttpGet("Auth")]
        public ActionResult Get(
            [FromHeader, Required(ErrorMessage = "Authorization header must not be blank")] string Authorization
            )
        {
            var response = _service.VerifyToken(Authorization);

            if (!response.Success)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, response);
            }

            return StatusCode((int)HttpStatusCode.OK, response);
        }

        [AllowAnonymous]
        [HttpPost("Auth")]
        public ActionResult Post(
            [FromHeader] string Authorization
            )
        {
            var response = _service.GenerateToken(Authorization);

            if (!response.Success)
            {
                return StatusCode((int)HttpStatusCode.Unauthorized, response);
            }

            return StatusCode((int)HttpStatusCode.OK, response);
        }
    }
}
