using EmployeeManagement.Application.Contracts.Repositories;
using EmployeeManagement.Application.Contracts.Services;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Exceptions;
using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Services
{
    public class EmployeeService:IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeService(IUnitOfWork UOW)
        {
            _unitOfWork = UOW;
        }
        public async Task<IEnumerable<EmployeeDTO>> GetAllAsync()
        {
           var employees= await _unitOfWork.Employees.GetAllAsync();
            return employees.Select(e => new EmployeeDTO
            {
                Id = e.Id,
                FullName=e.FirstName+""+e.LastName,
                Email=e.Email,
                Salary=e.Salary
                
            });

        }
        public async Task<PagedResult<EmployeeDTO> >GetPagedAsync(int pageNumber,int pageSize,decimal? minSalary,decimal? maxSalary, string? search, string? sortBy
            , string? sortOrder)
        
        {
            var query =  _unitOfWork.Employees.GetQueryable();
            if(minSalary.HasValue)
            {
                query = query.Where(q => q.Salary >= minSalary.Value);
            }
            if (maxSalary.HasValue)
            {
                query = query.Where(q => q.Salary <= maxSalary.Value);
            }
            if (!String.IsNullOrEmpty(search))
            {
                query=query.Where(q=>q.FirstName.Contains(search) || q.LastName.Contains(search) || q.Email.Contains(search));
            }
            var totalCount = await query.CountAsync();
            if(String.IsNullOrWhiteSpace(sortBy))
            {
                query = query.OrderBy(q => q.Id);
            }
            else
            {
                switch (sortBy.ToLower())
                {
                    case "salary":
                        query = sortOrder?.ToLower() == "desc" ? query.OrderByDescending(q => q.Salary) : query.OrderBy(q => q.Salary);
                        break;
                    case "firstname":
                        query = sortOrder?.ToLower() == "desc" ? query.OrderByDescending(q => q.FirstName) : query.OrderBy(q => q.FirstName);
                        break;
                    case "lastname":
                        query = sortOrder?.ToLower() == "desc" ? query.OrderByDescending(q => q.LastName) : query.OrderBy(q => q.LastName);
                        break;
                    default:
                        query.OrderBy(q => q.Id);
                        break;
                }
            }
                var skip = (pageNumber - 1) * pageSize;
            var employees=await query.Skip(skip).Take(pageSize).ToListAsync();
            var employeedtos= employees.Select(e => new EmployeeDTO
            {
                Id = e.Id,
                FullName = e.FirstName + " " + e.LastName,
                Email = e.Email,
                Salary = e.Salary

            });
            var result=new PagedResult<EmployeeDTO>();
            result.Items=employeedtos;
            result.TotalCount=totalCount;
            result.PageNumber = pageNumber;
            result.PageSize = pageSize;

            return result;

        }
        public async Task<EmployeeDTO?> GetByIdAsync(int id)
        {
            var employee= await _unitOfWork.Employees.GetByIdAsync(id);
            if (employee == null)
            {
                throw new NotFoundException($"Employee with id {id} was not found.");
            }
            return new EmployeeDTO
            {
                Id = employee.Id,
                FullName = employee.FirstName + "" + employee.LastName,
                Email = employee.Email,
                Salary = employee.Salary
            };
        }
        public async Task AddAsync(CreateEmployeeDTO dto)
        {
            var employee = new Employee
            {
                FirstName=dto.FirstName,
                LastName=dto.LastName,
                Email=dto.Email,
                Salary=dto.Salary
            };
            _unitOfWork.Employees.Add(employee);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task UpdateAsync(int id,UpdateEmployeeDTO dto)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);
            if (employee == null) { 
                throw new NotFoundException($"Employee with id {id} was not found.");
                }
            employee.FirstName = dto.FirstName;
            employee.LastName=dto.LastName;
            employee.Email= dto.Email;
            employee.Salary = dto.Salary;
            _unitOfWork.Employees.Update(employee);

            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var employee=await _unitOfWork.Employees.GetByIdAsync(id);
            if (employee == null)
            {
                throw new NotFoundException($"Employee with id {id} was not found.");

            }
            else
            {
                _unitOfWork.Employees.Delete(employee);
               await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
