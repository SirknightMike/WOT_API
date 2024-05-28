namespace wot_api.DTO
{
    public class PasswordEncryptionDTO
    {
        public string? HashPassword { get; set; }
        public byte[]? Salt { get; set; }
    }
}
