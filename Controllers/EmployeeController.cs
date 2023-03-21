using Microsoft.AspNetCore.Mvc;

namespace TinkeringAPI.Controllers;

public class EmployeeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}