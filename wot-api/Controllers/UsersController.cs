using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
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
                
                bool isValid = this.IsPasswordValid(user.Password, out string errorMessage);
                bool isEmailValid = this.IsEmailValid(user.Email, out errorMessage);

                if (!isValid || !isEmailValid)
                {
                    return BadRequest(errorMessage);
                }

                _userRepository.Add(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public IActionResult LoginUser([FromBody] Users user)
        {
            try
            {
                if (user == null)
                {
                    return NotFound();
                }

                var users = _userRepository.GetAll();

                foreach (var u in users)
                {
                    if (user.Email == u.Email && user.Password == u.Password)
                    {
                        return Ok();
                    }

                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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

        private Boolean IsEmailValid(string email, out string errorMessage)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                errorMessage = "Email is invalid";
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);

                errorMessage = null;
                return addr.Address == trimmedEmail;
            }
            catch
            {
                errorMessage = "Email is not valid";
                return false;
            }
        }




    }
}
