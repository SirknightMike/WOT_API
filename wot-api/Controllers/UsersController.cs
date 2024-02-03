using Microsoft.AspNetCore.Mvc;

namespace wot_api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        [HttpGet("test")]
        public ActionResult Index()
        {
            return Ok("Request is Successfull!");
        }
       
    }
}
