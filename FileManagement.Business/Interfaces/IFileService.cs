using FileManagement.Business.DTOs;

namespace FileManagement.Business.Interfaces
{
    public interface IFileService
    {
        Task<FileDto> CreateFileAsync(CreateFileRequest request, int userId);
        Task<List<FileDto>> GetFilesAsync(int userId);
        Task<List<PendingApprovalFileDto>> GetPendingApprovalsAsync(int userId);
        Task ApproveFileAsync(int fileId, int currentUserId);
        Task RejectFileAsync(int fileId, int currentUserId);
    }
} 