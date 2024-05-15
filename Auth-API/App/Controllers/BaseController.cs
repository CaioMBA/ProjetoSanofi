using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IBaseService _service;
        public BaseController(IBaseService service)
        {
            _service = service;
        }

        [HttpGet("/Base")]
        public async Task<ActionResult> Get(
            [FromHeader] string Header
            )
        {
            return StatusCode((int)HttpStatusCode.OK, new
            {
                RequestHeader = Header
            });
        }

        [HttpPost("/Base")]
        public async Task<ActionResult> Post(
            [FromBody] string Body
            )
        {
            return StatusCode((int)HttpStatusCode.OK, new
            {
                RequestBody = Body
            });
        }
    }
}
