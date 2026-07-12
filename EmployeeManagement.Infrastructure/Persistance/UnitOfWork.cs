using EmployeeManagement.Application.Contracts.Repositories;
using EmployeeManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Infrastructure.Persistance
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IEmployeeRepository Employees { get; private set; }
        public UnitOfWork(ApplicationDbContext context, IEmployeeRepository employeeRepository)
        {
            _context = context;
            Employees = employeeRepository;
        }
        public  Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
