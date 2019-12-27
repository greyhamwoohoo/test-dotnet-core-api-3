using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Yeha.Api.Models;

namespace Yeha.Api.Controllers
{
    [ApiController]
    [Route("api/ping")]
    [Authorize]
    public class PingAllowAnonymousController : ControllerBase
    {
        public PingAllowAnonymousController()
        {
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Product>> Get()
        {
            return Ok("pong");
        }

        [HttpDelete]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Product>> Delete()
        {
            return Ok("pong");
        }

        [HttpOptions]
        public ActionResult<IEnumerable<Product>> Options()
        {
            return Ok("pong");
        }

    }
}
