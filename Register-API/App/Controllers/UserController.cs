using Domain.Interfaces;
using Domain.Models;
using Domain.Models.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace App.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [RequiresClaim("CAD-USER", "True")]
        [HttpPost("Create")]
        public ActionResult Post(
            [FromBody, Required(ErrorMessage = "Body is required")] CreateUserRequestModel Body
            )
        {
            var response = _service.Create(Body);

            if (!response.Success)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, response);
            }

            return StatusCode((int)HttpStatusCode.OK, response);
        }
    }
}
