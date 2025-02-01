using System.Diagnostics;
using CarRental.Application.Dto.Queries.GetPopularCars;
using CarRental.Presentation.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Presentation.Controllers;

public class HomeController : Controller
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellation)
    {
        var popularCars = await _mediator.Send(new GetPopularCarsQuery(), cancellation); 

        return View(popularCars);
    }

    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    // return dynamic view error with no cache location

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpGet]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
