using FileManagement.Business.DTOs;

namespace FileManagement.Business.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
} 