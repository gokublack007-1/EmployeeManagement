using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Exceptions
{
    public class UserAlreadyExistsException:Exception
    {
        public UserAlreadyExistsException(string email):base($"A user with email '{email}'already exists.")
        { }
    }
}
