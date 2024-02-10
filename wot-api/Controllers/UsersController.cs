using Microsoft.AspNetCore.Mvc;
using wot_api.Entities;
using wot_api.Repositories.Interfaces;

namespace wot_api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public IActionResult RegisterUser([FromBody] Users user)
        {
            try
            {
                if(user == null)
                {
                    return BadRequest("Invalid user data");
                }
                _userRepository.Add(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("test")]
        public ActionResult Index()
        {
            return Ok("Request is Successfull!");
        }

        

       
    }
}
