using Microsoft.AspNetCore.Mvc;

using TinkeringAPI.Models;

namespace TinkeringAPI.Controllers;

public class EmployeeController : Controller
{
    private readonly EmployeeDbContext _context;
    
    //Get all employees
    public EmployeeController(EmployeeDbContext context)
    {
        _context = context;
    }

    [HttpGet("employees")]
    public IActionResult GetAllEmployees()
    {
        var employees = _context.Employees.ToList();
        return Ok(employees);
    }
    
    //Get employee by id
    [HttpGet("{id}")]
    public IActionResult GetEmployeeById(long id)
    {
        var employee = _context.Employees.FirstOrDefault(e => e.Id == id);
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee);
    }
    
    //Add new employee
    [HttpPost("employee/add")]
    public IActionResult AddEmployee([FromBody] Employee employee)
    {
        _context.Employees.Add(employee);
        _context.SaveChanges();
        return Ok(employee);
    }
    
    //Add bulk employees
    [HttpPost("employee/add-bulk")]
    public IActionResult AddBulkEmployees([FromBody] List<Employee> employees)
    {
        _context.Employees.AddRange(employees);
        _context.SaveChanges();
        return Ok(employees);
    }
    
    //Update employee
    [HttpPut("update/{id}")]
    public IActionResult UpdateEmployee(long id, [FromBody] Employee employee)
    {
        var employeeToUpdate = _context.Employees.FirstOrDefault(e => e.Id == id);
        if (employeeToUpdate == null)
        {
            return NotFound();
        }
        employeeToUpdate.Name = employee.Name;
        employeeToUpdate.Surname = employee.Surname;
        employeeToUpdate.Email = employee.Email;
        employeeToUpdate.Phone = employee.Phone;
        employeeToUpdate.Address = employee.Address;
        employeeToUpdate.DateOfBirth = employee.DateOfBirth;
        employeeToUpdate.DateOfEmployment = employee.DateOfEmployment;
        employeeToUpdate.DateOfDismissal = employee.DateOfDismissal;
        employeeToUpdate.Position = employee.Position;
        employeeToUpdate.Department = employee.Department;
        employeeToUpdate.Manager = employee.Manager;
        _context.SaveChanges();
        return Ok(employeeToUpdate);
    }
    
    //Delete employee
    [HttpDelete("employee/delete/{id}")]
    public IActionResult DeleteEmployee(long id)
    {
        var employeeToDelete = _context.Employees.FirstOrDefault(e => e.Id == id);
        if (employeeToDelete == null)
        {
            return NotFound();
        }
        _context.Employees.Remove(employeeToDelete);
        _context.SaveChanges();
        return Ok(employeeToDelete);
    }
    
    //Get all employees by department
    [HttpGet("department/{department}")]
    public IActionResult GetEmployeesByDepartment(string department)
    {
        var employees = _context.Employees.Where(e => e.Department == department).ToList();
        if (!employees.Any())
        {
            return NotFound();
        }
        return Ok(employees);
    }
    
    //Get all employees by position
    [HttpGet("position/{position}")]
    public IActionResult GetEmployeesByPosition(string position)
    {
        var employees = _context.Employees.Where(e => e.Position == position).ToList();
        if (!employees.Any())
        {
            return NotFound();
        }
        return Ok(employees);
    }
    
    //Get all employees by manager
    [HttpGet("manager/{manager}")]
    public IActionResult GetEmployeesByManager(string manager)
    {
        var employees = _context.Employees.Where(e => e.Manager == manager).ToList();
        if (!employees.Any())
        {
            return NotFound();
        }
        return Ok(employees);
    }
    
    //Get all employees by date of employment
    [HttpGet("dateOfEmployment/{dateOfEmployment}")]
    public IActionResult GetEmployeesByDateOfEmployment(DateTime dateOfEmployment)
    {
        var employees = _context.Employees.Where(e => e.DateOfEmployment == dateOfEmployment).ToList();
        if (!employees.Any())
        {
            return NotFound();
        }
        return Ok(employees);
    }
    
    //find employee by name
    [HttpGet("name/{name}")]
    public IActionResult GetEmployeeByName(string name)
    {
        var employee = _context.Employees.FirstOrDefault(e => e.Name == name);
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee);
    }
}