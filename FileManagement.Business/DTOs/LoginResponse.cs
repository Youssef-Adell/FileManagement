namespace FileManagement.Business.DTOs
{
    public class LoginResponse
    {
        public required string Token { get; set; }
        public required UserDto User { get; set; }
    }
} 