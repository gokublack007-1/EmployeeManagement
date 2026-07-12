using EmployeeManagement.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Validators
{
    public class UpdateEmployeeDTOValidator:AbstractValidator<UpdateEmployeeDTO>
    {
        public UpdateEmployeeDTOValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required").MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required").MaximumLength(100);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Salary).GreaterThan(0).WithMessage("Salary must be greater than zero");


        }
    }
}
