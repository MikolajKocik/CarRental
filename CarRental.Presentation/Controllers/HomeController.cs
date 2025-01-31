using System.Diagnostics;
using CarRental.Infrastructure.Data;
using CarRental.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Presentation.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger; 
    private readonly ApplicationDbContext _context; 

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger; 
        _context = context; 
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Return 3 popular cars
        var cars = await _context.Cars.OrderBy(c => c.Id).Take(3).ToListAsync();
        return View(cars); 
    }

    [HttpGet]
    public IActionResult Privacy()
    {
        return View();  
    }

    // TODO

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
