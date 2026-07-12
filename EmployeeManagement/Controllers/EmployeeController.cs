using EmployeeManagement.API.Filters;
using EmployeeManagement.Application.Contracts.Services;
using EmployeeManagement.Application.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IValidator<CreateEmployeeDTO> _validator;
        private readonly IValidator<UpdateEmployeeDTO> _updateEmployeeDTOValidator;
        public EmployeeController(IEmployeeService employeeService,IValidator<CreateEmployeeDTO> validator ,IValidator<UpdateEmployeeDTO> updateEmployeeDTOValidator)
        {
            _employeeService = employeeService;
            _validator = validator;
            _updateEmployeeDTOValidator = updateEmployeeDTOValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _employeeService.GetAllAsync();

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilter<CreateEmployeeDTO>))]
        public async Task<IActionResult> Create(CreateEmployeeDTO employeeDto)
        {
            var result = await _validator.ValidateAsync(employeeDto);
            await _employeeService.AddAsync(employeeDto);

            return Ok();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilter<UpdateEmployeeDTO>))]
        public async Task<IActionResult> Update(
            int id,
            UpdateEmployeeDTO employeeDto)
        {
            var result = await _updateEmployeeDTOValidator.ValidateAsync(employeeDto);
            await _employeeService.UpdateAsync(id, employeeDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeService.DeleteAsync(id);

            return NoContent();
        }
    }
}
