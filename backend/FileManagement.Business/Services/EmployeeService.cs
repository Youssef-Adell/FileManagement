using FileManagement.Business.DTOs;
using FileManagement.Business.Interfaces;
using FileManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace FileManagement.Business.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeDto>> GetEmployeesAsync()
        {
            return await _context.Users
                .Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    FullName = e.FullName
                })
                .ToListAsync();
        }
    }
} 