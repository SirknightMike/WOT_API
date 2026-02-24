using Microsoft.AspNetCore.Mvc;
using wot_api.DTO;
using wot_api.Services;

namespace wot_api.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserAuthService _userAuthService;

        public UsersController(IUserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequestDTO user)
        {
            try
            {
                var result = await _userAuthService.RegisterAsync(user);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.ErrorMessage);
                }

                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserRequestDTO user)
        {
            try
            {
                var result = await _userAuthService.LoginAsync(user);
                if (!result.IsSuccess)
                {
                    if (result.ErrorType == AuthErrorType.Validation)
                    {
                        return BadRequest(result.ErrorMessage);
                    }

                    return Unauthorized(result.ErrorMessage);
                }

                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
