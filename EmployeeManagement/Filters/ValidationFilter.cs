
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeManagement.API.Filters
{
    public class ValidationFilter<T> : IAsyncActionFilter
        where T : class
    {
        public readonly IValidator<T> _validator;
        public ValidationFilter(IValidator<T> validator)
        {

            _validator = validator;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var model = context.ActionArguments.Values.OfType<T>().FirstOrDefault();
            if (model == null)
            {
                await next();
                return;
            }
            var validationResult= await _validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                context.Result = new BadRequestObjectResult(validationResult.Errors);
                return;
            }
            await next();
        }
    }
}
