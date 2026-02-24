using wot_api.Entities;

namespace wot_api.DTO
{
    public class RegisterUserResponseDTO
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public UserType? UserTypeId { get; set; }
    }
}
