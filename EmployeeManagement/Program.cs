using EmployeeManagement.API.Filters;
using EmployeeManagement.API.Middlewares;
using EmployeeManagement.Application.Contracts.Repositories;
using EmployeeManagement.Application.Contracts.Services;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Application.Validators;
using EmployeeManagement.Infrastructure.Data;
using EmployeeManagement.Infrastructure.Persistance;
using EmployeeManagement.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        );
});
builder.Services.AddValidatorsFromAssemblyContaining<CreateEmployeeDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateEmployeeDTOValidator>();
builder.Services.AddScoped(typeof(ValidationFilter<>));
builder.Services.AddScoped<IEmployeeRepository,EmployeeRepository>();
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<IEmployeeService,EmployeeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();
