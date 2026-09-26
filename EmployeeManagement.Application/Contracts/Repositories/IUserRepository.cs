using EmployeeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Contracts.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        void Add(User user);

    }
}
