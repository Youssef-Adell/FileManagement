using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FileManagement.Business.DTOs
{
    public class CreateFileRequest
    {
        [Required]
        [MaxLength(200)]
        public required string Subject { get; set; }

        [Required]
        public required int ClassificationId { get; set; }

        [Required]
        public required int ResponsibleEmployeeId { get; set; }

        public required IFormFile Attachment { get; set; }
    }
} 