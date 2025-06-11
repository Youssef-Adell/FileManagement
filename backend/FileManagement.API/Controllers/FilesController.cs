using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FileManagement.Business.DTOs;
using FileManagement.Business.Interfaces;
using FileManagement.Data.Models;

namespace FileManagement.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        [Authorize(Roles = "Creator")]
        public async Task<ActionResult<FileDto>> CreateFile([FromForm] CreateFileRequest request)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                var file = await _fileService.CreateFileAsync(request, currentUserId);
                return Ok(file);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Creator")]
        public async Task<ActionResult<List<FileDto>>> GetFiles()
        {
            var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var files = await _fileService.GetFilesAsync(currentUserId);
            return Ok(files);
        }

        [HttpGet("pending-approvals")]
        [Authorize(Roles = "Approver")]
        public async Task<ActionResult<List<PendingApprovalFileDto>>> GetPendingApprovals()
        {
            var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var approvals = await _fileService.GetPendingApprovalsAsync(currentUserId);
            return Ok(approvals);
        }

        [HttpPost("{fileId}/approve")]
        [Authorize(Roles = "Approver")]
        public async Task<ActionResult> ApproveFile(int fileId)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                await _fileService.ApproveFileAsync(fileId, currentUserId);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{fileId}/reject")]
        [Authorize(Roles = "Approver")]
        public async Task<ActionResult> RejectFile(int fileId)
        {
            try
            {
                var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
                await _fileService.RejectFileAsync(fileId, currentUserId);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 