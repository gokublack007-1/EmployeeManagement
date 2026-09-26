using EmployeeManagement.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Validators
{
public class RegisterDTOValidator:AbstractValidator<RegisterDTO>
    {
        public RegisterDTOValidator() 
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required").MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(50).Matches("[A-Z]")
                .WithMessage("Password must contain atleast one uppercase letter").Matches("[a-z]")
                .WithMessage("Password must contain atleast one lowercase letter").Matches("[0-9]")
                .WithMessage("Password must contain atleast one number");
        }
    }
}
