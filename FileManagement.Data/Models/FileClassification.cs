using System.ComponentModel.DataAnnotations;

namespace FileManagement.Data.Models
{
    public class FileClassification
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        
        // Navigation properties
        public ICollection<FileEntity> Files { get; set; } = new List<FileEntity>();
    }
} 