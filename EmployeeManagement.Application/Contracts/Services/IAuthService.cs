using EmployeeManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Contracts.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDTO dto);
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
