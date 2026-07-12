using EmployeeManagement.API.Models;
using EmployeeManagement.Application.Exceptions;
using System.Text.Json;

namespace EmployeeManagement.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _enviroment;
        public ExceptionHandlingMiddleware(RequestDelegate next, IWebHostEnvironment enviroment)
        {
            _next = next;
            _enviroment = enviroment;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
            await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
            var response = new ErrorResponse();
            response.StatusCode = context.Response.StatusCode;
            response.Message =
              ex switch
              {
                  NotFoundException => ex.Message,
                  _ => _enviroment.IsDevelopment() ? ex.Message : "An unexpected error occoured."
              };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
