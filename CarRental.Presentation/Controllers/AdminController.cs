using AutoMapper;
using CarRental.Application.CQRS.Commands.Reservation.ConfirmReservation;
using CarRental.Application.CQRS.Queries.AdminQueries;
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
    public async Task<IActionResult> ConfirmReservation(int reservationId)
    {

        var confirmed = await _mediator.Send(new ConfirmReservationCommand
        {
            ReservationId = reservationId
        });

        return RedirectToAction(nameof(Reports));
    }
}

