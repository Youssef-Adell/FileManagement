using FileManagement.Business.DTOs;

namespace FileManagement.Business.Interfaces
{
    public interface IClassificationService
    {
        Task<List<ClassificationDto>> GetClassificationsAsync();
    }
} 