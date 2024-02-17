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
                
                bool isValid = this.IsPasswordValid(user.Password, out string errroMessage);

                if (!isValid)
                {
                    return BadRequest(errroMessage);
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

        private Boolean IsPasswordValid(string password, out string errorMessage)
        {
            var passwordLength = password.Length;
            var hasSpecialCharacters = password.Any(ch => ! char.IsLetterOrDigit(ch));
            errorMessage = null;

            if(passwordLength < 6)
            {
                errorMessage = "Please enter a password which contains more than 5 characters.";
                return false;
            } 
            else if (!hasSpecialCharacters)
            {
                errorMessage = "Password should contain at least one speacial character '@#$%^&*'";
                return false;
            }

            return true;
        }

        

       
    }
}
