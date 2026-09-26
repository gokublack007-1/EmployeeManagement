using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Contracts.Services
{
    public interface IpasswordHasher
    {
        string Hash(string password);
        bool Verify(string password,string hash);
    }
}
