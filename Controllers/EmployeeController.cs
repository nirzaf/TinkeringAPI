using System.Globalization;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await _context.Employees.ToListAsync();
        return Ok(employees);
    }

    // Get employee by id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeByIdAsync(long id)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

  //Add new employee
[HttpPost("employee/add")]
public async Task<IActionResult> AddEmployeeAsync([FromBody] CreateEmployeeObject? employee)
{
    if(employee == null)
    {
        return BadRequest("Employee is null.");
    }

    var employeeToAdd = new Employee
    {
        Name = employee.Name,
        Surname = employee.Surname,
        Email = employee.Email,
        Phone = employee.Phone,
        Address = employee.Address,
        Position = employee.Position,
        Department = employee.Department,
        Manager = employee.Manager
    };
    
    //Valid format YYYY/MM/DD hh:mm:ss

    if (DateOnly.TryParse(employee.DateOfBirth, out DateOnly dateOfBirth))
    {
        // convert to dateOfBirth from DateOnly to DateTime
        employeeToAdd.DateOfBirth = new DateTime(dateOfBirth.Year, dateOfBirth.Month, dateOfBirth.Day);
    }
     
    // convert to dateOnly field in employee.DateOfBirth field format is in MM/dd/yyyy HH:mm:ss
    else
    {
        if (employee.DateOfBirth is { Length: > 10 })
        {
            employee.DateOfBirth = employee.DateOfBirth.Substring(0, 10);
        }

        if (DateOnly.TryParseExact(employee.DateOfBirth,
                new[]
                {
                    "mm/DD/yyyy HH:mm:ss ", "MM/dd/yyyy", "dd/MM/yyyy", "yyyy/MM/dd", "yyyy/dd/MM", "dd MMMM yyyy", "dd MMM yyyy", "dd MMMM yy",
                    "dd MMM yy", "dd/MM/yyyy hh:mm tt", "yyyy/MM/dd hh:mm tt"
                }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth))
        {
            employeeToAdd.DateOfBirth = new DateTime(dateOfBirth.Year, dateOfBirth.Month, dateOfBirth.Day);
        }
        else
        {
            return BadRequest(
                "Invalid DateOfBirth format. Supported formats: MM/dd/yyyy, dd/MM/yyyy, yyyy/MM/dd, yyyy/dd/MM, dd MMMM yyyy, dd MMM yyyy, dd MMMM yy, dd MMM yy, dd/MM/yyyy h:mm tt, yyyy/MM/dd h:mm tt.");
        }
    }
    
    if(employee.DateOfEmployment is { Length: > 10 })
    {
        employee.DateOfEmployment = employee.DateOfEmployment.Substring(0, 10);
    }
    else
    {

        if (DateOnly.TryParse(employee.DateOfEmployment.ToString(CultureInfo.CurrentCulture),
                out DateOnly dateOfEmployment))
        {
            employeeToAdd.DateOfEmployment =  new DateTime(dateOfEmployment.Year, dateOfEmployment.Month, dateOfEmployment.Day);
        }
        else
        {
            if (DateOnly.TryParseExact(employee.DateOfEmployment.ToString(CultureInfo.CurrentCulture),
                    new[]
                    {
                        "MM/dd/yyyy", "dd/MM/yyyy", "yyyy/MM/dd", "yyyy/dd/MM", "dd MMMM yyyy", "dd MMM yyyy",
                        "dd MMMM yy",
                        "dd MMM yy", "dd/MM/yyyy hh:mm tt", "yyyy/MM/dd hh:mm tt"
                    }, CultureInfo.CurrentCulture, DateTimeStyles.None, out dateOfEmployment))
            {
                employeeToAdd.DateOfEmployment = new DateTime(dateOfEmployment.Year, dateOfEmployment.Month, dateOfEmployment.Day);
            }
            else
            {
                return BadRequest(
                    "Invalid DateOfEmployment format. Supported formats: MM/dd/yyyy, dd/MM/yyyy, yyyy/MM/dd, yyyy/dd/MM, dd MMMM yyyy, dd MMM yyyy, dd MMMM yy, dd MMM yy, dd/MM/yyyy h:mm tt, yyyy/MM/dd h:mm tt.");
            }
        }
    }
    
    if (employee.DateOfDismissal is { Length: > 10 })
    {
        employee.DateOfDismissal = employee.DateOfDismissal.Substring(0, 10);
    }
    else
    {

        if (DateOnly.TryParse(employee.DateOfDismissal.ToString(CultureInfo.CurrentCulture),
                out DateOnly dateOfDismissal))
        {
            employeeToAdd.DateOfDismissal = new DateTime(dateOfDismissal.Year, dateOfDismissal.Month, dateOfDismissal.Day);
        }
        else
        {
            if (DateOnly.TryParseExact(employee.DateOfDismissal.ToString(CultureInfo.CurrentCulture),
                    new[]
                    {
                        "MM/dd/yyyy", "dd/MM/yyyy", "yyyy/MM/dd", "yyyy/dd/MM", "dd MMMM yyyy", "dd MMM yyyy",
                        "dd MMMM yy",
                        "dd MMM yy", "dd/MM/yyyy hh:mm tt", "yyyy/MM/dd hh:mm tt"
                    }, CultureInfo.CurrentCulture, DateTimeStyles.None, out dateOfDismissal))
            {
                employeeToAdd.DateOfDismissal = new DateTime(dateOfDismissal.Year, dateOfDismissal.Month, dateOfDismissal.Day);
            }
            else
            {
                return BadRequest(
                    "Invalid DateOfDismissal format. Supported formats: MM/dd/yyyy, dd/MM/yyyy, yyyy/MM/dd, yyyy/dd/MM, dd MMMM yyyy, dd MMM yyyy, dd MMMM yy, dd MMM yy, dd/MM/yyyy h:mm tt, yyyy/MM/dd h:mm tt.");
            }
        }
    }

    await _context.Employees.AddAsync(employeeToAdd);
    await _context.SaveChangesAsync();
    return Ok(employeeToAdd);
}


    // Add bulk employees
    [HttpPost("employee/add-bulk")]
    public async Task<IActionResult> AddBulkEmployeesAsync([FromBody] List<Employee> employees)
    {
        _context.Employees.AddRange(employees);
        await _context.SaveChangesAsync();
        return Ok(employees);
    }

    // Update employee
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateEmployeeAsync(long id, [FromBody] Employee employee)
    {
        var employeeToUpdate = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
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
        await _context.SaveChangesAsync();
        return Ok(employeeToUpdate);
    }

    // Delete employee
    [HttpDelete("employee/delete/{id}")]
    public async Task<IActionResult> DeleteEmployeeAsync(long id)
    {
        var employeeToDelete = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        if (employeeToDelete == null)
        {
            return NotFound();
        }

        _context.Employees.Remove(employeeToDelete);
        await _context.SaveChangesAsync();
        return Ok(employeeToDelete);
    }

    // Get all employees by department
    [HttpGet("department/{department}")]
    public async Task<IActionResult> GetEmployeesByDepartmentAsync(string department)
    {
        var employees = await _context.Employees.Where(e => e.Department == department).ToListAsync();
        if (!employees.Any())
        {
            return NotFound();
        }

        return Ok(employees);
    }

    // Get all employees by position
    [HttpGet("position/{position}")]
    public async Task<IActionResult> GetEmployeesByPositionAsync(string position)
    {
        var employees = await _context.Employees.Where(e => e.Position == position).ToListAsync();
        if (!employees.Any())
        {
            return NotFound();
        }

        return Ok(employees);
    }

    // Get all employees by manager
    [HttpGet("manager/{manager}")]
    public async Task<IActionResult> GetEmployeesByManagerAsync(string manager)
    {
        var employees = await _context.Employees.Where(e => e.Manager == manager).ToListAsync();
        if (!employees.Any())
        {
            return NotFound();
        }

        return Ok(employees);
    }

    // Get all employees by date of employment
    [HttpGet("dateOfEmployment/{dateOfEmployment}")]
    public async Task<IActionResult> GetEmployeesByDateOfEmploymentAsync(DateOnly dateOfEmployment)
    {
        var doe = new DateTime(dateOfEmployment.Year, dateOfEmployment.Month, dateOfEmployment.Day);
        var employees = await _context.Employees.Where(e => e.DateOfEmployment == doe).ToListAsync();
        if (!employees.Any())
        {
            return NotFound();
        }

        return Ok(employees);
    }

    // Find employee by name
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetEmployeeByNameAsync(string name)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Name == name);
        if (employee == null)
        {
            return NotFound();
        }

        return Ok(employee);
    }
}