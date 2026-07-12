using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Contracts.Repositories
{
    public interface IUnitOfWork
    {
        IEmployeeRepository Employees { get; }
        Task<int> SaveChangesAsync();
    }
}
