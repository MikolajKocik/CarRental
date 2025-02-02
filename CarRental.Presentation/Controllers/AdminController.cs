using AutoMapper;
using CarRental.Application.Dto;
using CarRental.Application.Dto.ConfirmReservation;
using CarRental.Application.Dto.CreateCar;
using CarRental.Application.Dto.Queries.AdminQueries;
using CarRental.Presentation.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarRental.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AdminController (ILogger<AdminController> logger, IMediator mediator, IMapper mapper)
    {
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Reports()
    {
        try
        {
            var report = await _mediator.Send(new GetReportQuery());

            var viewModel = _mapper.Map<AdminReportsViewModel>(report); // reportDto -> viewModel

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching admin reports.");
            return View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken] 
    public async Task<IActionResult> ConfirmReservation()
    {
        var confirmed = await _mediator.Send(new ConfirmReservationCommand());
        return RedirectToAction(nameof(Reports));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCarViewModel carViewModel, IFormFile image,
        CancellationToken cancellation)
    {
        if (ModelState.IsValid)
        {
            var command = new CreateCarCommand
            {
                Car = _mapper.Map<CarDto>(carViewModel),
                Image = image
            };

            await _mediator.Send(command, cancellation);
            return RedirectToAction("Index");
        }

        return View(carViewModel);
    }

}

