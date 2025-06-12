using Microsoft.EntityFrameworkCore;
using FileManagement.Business.DTOs;
using FileManagement.Data.Models;
using FileManagement.Business.Interfaces;
using FileManagement.Data;

namespace FileManagement.Business.Services
{
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;

        public FileService(ApplicationDbContext context, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<FileDto> CreateFileAsync(CreateFileRequest request, int currentUserId)
        {
            // Validate classification exists
            var classificationExists = await _context.FileClassifications
                .AnyAsync(c => c.Id == request.ClassificationId);
            if (!classificationExists)
                throw new ArgumentException("Invalid classification ID");

            // Validate responsible employee exists
            var responsibleEmployeeExists = await _context.Users
                .AnyAsync(u => u.Id == request.ResponsibleEmployeeId);
            if (!responsibleEmployeeExists)
                throw new ArgumentException("Invalid responsible employee ID");

            // Validate and upload attachment
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx", ".xls", ".xlsx" };
            var fileExtension = Path.GetExtension(request.Attachment.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
                throw new ArgumentException("Invalid file type");

            var attachmentUrl = await _cloudinaryService.UploadFileAsync(request.Attachment);

            // Generate file number
            var fileCount = await _context.Files.CountAsync();
            var fileNumber = $"FILE-{DateTime.Now.Year}-{(fileCount + 1):D3}";

            // Create file
            var file = new FileEntity
            {
                FileNumber = fileNumber,
                Subject = request.Subject,
                SubmitterId = currentUserId,
                Status = FileStatus.Pending,
                FileDate = DateTime.UtcNow,
                ClassificationId = request.ClassificationId,
                ResponsibleEmployeeId = request.ResponsibleEmployeeId,
                AttachmentUrl = attachmentUrl
            };

            _context.Files.Add(file);

            // Create approval records
            var approvals = new List<FileApproval>
            {
                new FileApproval
                {
                    File = file,
                    ApproverId = 2, // Employee 2
                    ApprovalOrder = 1,
                    Status = ApprovalStatus.Pending
                },
                new FileApproval
                {
                    File = file,
                    ApproverId = 3, // Employee 3
                    ApprovalOrder = 2,
                    Status = ApprovalStatus.Pending
                }
            };

            _context.FileApprovals.AddRange(approvals);
            await _context.SaveChangesAsync();

            // Return file DTO using direct projection
            return await _context.Files
                .Where(f => f.Id == file.Id)
                .Select(f => new FileDto
                {
                    Id = f.Id,
                    FileNumber = f.FileNumber,
                    Subject = f.Subject,
                    SubmitterName = f.Submitter!.FullName,
                    Status = f.Status.ToString(),
                    FileDate = f.FileDate,
                    ClassificationName = f.Classification!.Name,
                    ResponsibleEmployeeName = f.ResponsibleEmployee!.FullName,
                    AttachmentUrl = f.AttachmentUrl
                })
                .FirstAsync();
        }

        public async Task<List<FileDto>> GetFilesAsync(int userId)
        {
            var files = await _context.Files
                .Where(f => f.SubmitterId == userId)
                .Select(f => new FileDto
                {
                    Id = f.Id,
                    FileNumber = f.FileNumber,
                    Subject = f.Subject,
                    SubmitterName = f.Submitter!.FullName,
                    Status = f.Status.ToString(),
                    FileDate = f.FileDate,
                    ClassificationName = f.Classification!.Name,
                    ResponsibleEmployeeName = f.ResponsibleEmployee!.FullName,
                    AttachmentUrl = f.AttachmentUrl
                })
                .ToListAsync();

            return files;
        }

        public async Task<List<PendingApprovalFileDto>> GetPendingApprovalsAsync(int userId)
        {
            var approvals = await _context.FileApprovals
                .Where(a => a.ApproverId == userId && a.Status == ApprovalStatus.Pending)
                .Where(a => a.ApprovalOrder == 1 ||
                        (a.ApprovalOrder == 2 &&
                        _context.FileApprovals.Any(prev => prev.FileId == a.FileId &&
                                                            prev.ApprovalOrder == 1 &&
                                                            prev.Status == ApprovalStatus.Approved)))
                .Select(a => new PendingApprovalFileDto
                {
                    Id = a.FileId,
                    FileNumber = a.File!.FileNumber,
                    Subject = a.File!.Subject,
                    SubmitterName = a.File!.Submitter!.FullName,
                    FileDate = a.File!.FileDate,
                    ClassificationName = a.File!.Classification!.Name,
                    AttachmentUrl = a.File!.AttachmentUrl
                })
                .ToListAsync();

            return approvals;
        }

        public async Task ApproveFileAsync(int fileId, int currentUserId)
        {
            var approval = await _context.FileApprovals
                .Include(a => a.File)
                .FirstOrDefaultAsync(a => a.FileId == fileId && a.ApproverId == currentUserId && a.Status == ApprovalStatus.Pending);

            if (approval == null)
                throw new UnauthorizedAccessException("You are not authorized to approve this file");

            // Check if it's the correct approval order
            if (approval.ApprovalOrder == 2)
            {
                var previousApproval = await _context.FileApprovals
                    .FirstOrDefaultAsync(a => a.FileId == fileId && a.ApprovalOrder == 1);
                if (previousApproval?.Status != ApprovalStatus.Approved)
                    throw new InvalidOperationException("File cannot be approved. Previous approver has not approved yet.");
            }

            // Update approval
            approval.Status = ApprovalStatus.Approved;

            // Check if this is the final approval
            var allApprovals = await _context.FileApprovals
                .Where(a => a.FileId == fileId)
                .ToListAsync();

            var newStatus = allApprovals.All(a => a.Status == ApprovalStatus.Approved || a.Id == approval.Id)
                ? FileStatus.Approved
                : FileStatus.Pending;

            // Update file status
            approval.File!.Status = newStatus;

            await _context.SaveChangesAsync();
        }

        public async Task RejectFileAsync(int fileId, int currentUserId)
        {
            var approval = await _context.FileApprovals
                .Include(a => a.File)
                .FirstOrDefaultAsync(a => a.FileId == fileId && a.ApproverId == currentUserId && a.Status == ApprovalStatus.Pending);

            if (approval == null)
                throw new UnauthorizedAccessException("You are not authorized to reject this file");

            // Check if it's the correct approval order for rejection
            if (approval.ApprovalOrder == 2)
            {
                var previousApproval = await _context.FileApprovals
                    .FirstOrDefaultAsync(a => a.FileId == fileId && a.ApprovalOrder == 1);
                if (previousApproval?.Status != ApprovalStatus.Approved)
                    throw new InvalidOperationException("File cannot be rejected. Previous approver has not approved yet.");
            }

            // Update approval
            approval.Status = ApprovalStatus.Rejected;

            // Update file status to rejected
            approval.File!.Status = FileStatus.Rejected;

            await _context.SaveChangesAsync();
        }
    }
}