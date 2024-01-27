using Microsoft.AspNetCore.Mvc;

namespace wot_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            return Ok();
        }
       
    }
}
