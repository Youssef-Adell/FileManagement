using Microsoft.AspNetCore.Http;

namespace FileManagement.Business.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadFileAsync(IFormFile file);
    }
} 