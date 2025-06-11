using FileManagement.Business.DTOs;

namespace FileManagement.Business.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetEmployeesAsync();
    }
} 