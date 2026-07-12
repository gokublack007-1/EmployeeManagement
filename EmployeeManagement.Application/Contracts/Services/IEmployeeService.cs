using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Contracts.Services
{
    public interface IEmployeeService

    {
        Task<IEnumerable<EmployeeDTO>> GetAllAsync();
        Task<EmployeeDTO?> GetByIdAsync(int id);
        Task AddAsync(CreateEmployeeDTO employee);
        Task UpdateAsync(int id, UpdateEmployeeDTO employee);
        Task DeleteAsync(int id);
    }
}
