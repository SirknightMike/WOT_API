using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using wot_api.Classes;
using wot_api.Data;
using wot_api.DTO;
using wot_api.Entities;

namespace wot_api.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly DataContext _context;
        private readonly AuthService _authService;
        private readonly DataProtection _dataProtection;

        public UserAuthService(DataContext context, AuthService authService, DataProtection dataProtection)
        {
            _context = context;
            _authService = authService;
            _dataProtection = dataProtection;
        }

        public async Task<AuthOperationResult<RegisterUserResponseDTO>> RegisterAsync(RegisterUserRequestDTO request)
        {
            if (request == null)
            {
                return AuthOperationResult<RegisterUserResponseDTO>.ValidationFailure("Invalid user data");
            }

            var password = request.Password ?? string.Empty;
            var email = request.Email ?? string.Empty;

            if (!IsPasswordValid(password, out var passwordError))
            {
                return AuthOperationResult<RegisterUserResponseDTO>.ValidationFailure(passwordError!);
            }

            if (!IsEmailValid(email, out var emailError))
            {
                return AuthOperationResult<RegisterUserResponseDTO>.ValidationFailure(emailError!);
            }

            if (await IsDuplicateUserAsync(email))
            {
                return AuthOperationResult<RegisterUserResponseDTO>.ValidationFailure("User already exists.");
            }

            var user = new User
            {
                Username = request.Username,
                Email = email.Trim(),
                Password = password,
                UserTypeId = UserType.FreeUser
            };

            var passwordEncryption = _dataProtection.HashPassword(user);
            user.Password = passwordEncryption.HashPassword;
            user.Salt = passwordEncryption.Salt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var response = new RegisterUserResponseDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                UserTypeId = user.UserTypeId
            };

            return AuthOperationResult<RegisterUserResponseDTO>.Success(response);
        }

        public async Task<AuthOperationResult<AuthResponseDTO>> LoginAsync(LoginUserRequestDTO request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return AuthOperationResult<AuthResponseDTO>.ValidationFailure("Email and password are required");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser == null || existingUser.Salt == null || string.IsNullOrWhiteSpace(existingUser.Password))
            {
                return AuthOperationResult<AuthResponseDTO>.UnauthorizedFailure("Invalid email or password");
            }

            var computedHash = _dataProtection.VerifyHashPassword(request.Password, existingUser.Salt);
            if (computedHash != existingUser.Password)
            {
                return AuthOperationResult<AuthResponseDTO>.UnauthorizedFailure("Invalid email or password");
            }

            var token = _authService.GenerateJwtToken(existingUser.Email ?? string.Empty);
            return AuthOperationResult<AuthResponseDTO>.Success(new AuthResponseDTO
            {
                Token = token
            });
        }

        private static bool IsPasswordValid(string password, out string? errorPasswordMessage)
        {
            var passwordLength = password.Length;
            var hasSpecialCharacters = password.Any(ch => !char.IsLetterOrDigit(ch));
            errorPasswordMessage = null;

            if (passwordLength < 6)
            {
                errorPasswordMessage = "Please enter a password which contains more than 5 characters.";
                return false;
            }

            if (!hasSpecialCharacters)
            {
                errorPasswordMessage = "Password should contain at least one speacial character '@#$%^&*'";
                return false;
            }

            return true;
        }

        private static bool IsEmailValid(string email, out string? errorEmailMessage)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                errorEmailMessage = "Email is required";
                return false;
            }

            var trimmedEmail = email.Trim();
            if (trimmedEmail.EndsWith("."))
            {
                errorEmailMessage = "Email is invalid";
                return false;
            }

            try
            {
                var addr = new MailAddress(email);
                errorEmailMessage = null;
                return addr.Address.ToLowerInvariant() == trimmedEmail.ToLowerInvariant();
            }
            catch
            {
                errorEmailMessage = "Email is not valid";
                return false;
            }
        }

        private Task<bool> IsDuplicateUserAsync(string email)
        {
            return _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
