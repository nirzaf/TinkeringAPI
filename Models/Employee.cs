using System.ComponentModel.DataAnnotations;

namespace TinkeringAPI.Models;

public class Employee
{
    [Key]
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime DateOfEmployment { get; set; }
    public DateTime DateOfDismissal { get; set; }
    public string? Position { get; set; }
    public string? Department { get; set; }
    public string? Manager { get; set; }
}