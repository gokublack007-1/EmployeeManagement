using EmployeeManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Contracts.Services
{
    public interface IJWTTokenService
    {
        string GenerateToken(User user);
    }
}
