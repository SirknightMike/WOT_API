using wot_api.DTO;

namespace wot_api.Services
{
    public interface IUserAuthService
    {
        Task<AuthOperationResult<RegisterUserResponseDTO>> RegisterAsync(RegisterUserRequestDTO request);
        Task<AuthOperationResult<AuthResponseDTO>> LoginAsync(LoginUserRequestDTO request);
    }
}
