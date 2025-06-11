using FileManagement.Business.DTOs;
using FileManagement.Business.Interfaces;
using FileManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace FileManagement.Business.Services
{
    public class ClassificationService : IClassificationService
    {
        private readonly ApplicationDbContext _context;

        public ClassificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClassificationDto>> GetClassificationsAsync()
        {
            return await _context.FileClassifications
                .Select(c => new ClassificationDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }
    }
} 