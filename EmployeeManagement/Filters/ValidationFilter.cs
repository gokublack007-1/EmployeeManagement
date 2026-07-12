
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
                var errors = validationResult.Errors.GroupBy(e => e.PropertyName).ToDictionary(g => g.Key,
                    g => g.Select(x => x.ErrorMessage));
                context.Result = new BadRequestObjectResult(errors);
                return;
            }
            await next();
        }
    }
}
