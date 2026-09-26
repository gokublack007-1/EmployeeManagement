using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.DTOs
{
    public class RegisterDTO
    {
        public string UserName { get; set; } = null!;
        public string Password { get; set; }= null!;
        public string Email { get; set; } = null!;
    }
}
