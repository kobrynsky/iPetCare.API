using System.Net;
using Application.Services.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult SendResponse(ServiceResponse response)
        {
            switch (response.ResponseType)
            {
                case HttpStatusCode.OK:
                    return Ok();
                case HttpStatusCode.Unauthorized:
                    return Unauthorized();
                case HttpStatusCode.Forbidden:
                    return Forbid();
                case HttpStatusCode.NotFound:
                    return NotFound();
                default:
                    return BadRequest(response.Message);
            }
        }

        protected IActionResult SendResponse<T>(ServiceResponse<T> response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(response.ResponseContent);
                case HttpStatusCode.Unauthorized:
                    return Unauthorized();
                case HttpStatusCode.NotFound:
                    return NotFound();
                case HttpStatusCode.Forbidden:
                    return Forbid();
                default:
                    return BadRequest(response.Message);
            }
        }
    }
}