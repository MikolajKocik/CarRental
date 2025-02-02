using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using CarRental.Presentation.Models;
using AutoMapper;
using CarRental.Application.Dto.CreateReservation;
using CarRental.Application.Dto.Queries.ReservationQueries.GetReservationDetails;
using CarRental.Application.Dto.Queries.ReservationQueries.GetMyReservations;

namespace CarRental.Presentation.Controllers;

[Authorize] 
public class ReservationController : Controller
{
    private readonly UserManager<IdentityUser> _userManager; 
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ReservationController(UserManager<IdentityUser> userManager,
        ILogger<ReservationController> logger, IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _userManager = userManager;
        _mapper = mapper;
    }
    

    [HttpGet]
    public async Task<IActionResult> MyReservations(CancellationToken cancellation)
    {
        var reservations = await _mediator.Send(new GetMyReservationQuery(), cancellation);

        return View(reservations);
    }

    
    [HttpGet]
    public IActionResult Create(int carId, CreateReservationViewModel createReservationViewModel)
    {
        createReservationViewModel = new CreateReservationViewModel
        {
            CarId = carId, 
            StartDate = DateTime.Today, 
            EndDate = DateTime.Today.AddDays(1) 
        }; 
        return View(createReservationViewModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateReservationViewModel createReservationViewModel,
        CancellationToken cancellation)
    {

        if (!ModelState.IsValid)
        {    
            return View(createReservationViewModel);
        }

        try
        {
            // ViewModel -> Command
            var command = new CreateReservationCommand
            {
                CarId = createReservationViewModel.CarId,
                StartDate = createReservationViewModel.StartDate,
                EndDate = createReservationViewModel.EndDate
            };

            await _mediator.Send(command, cancellation);
            return RedirectToAction(nameof(MyReservations));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(createReservationViewModel);
        }
    }

 
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var reservation = await _mediator.Send(new GetReservationDetailsQuery { Id = id });

        return View(reservation);
    }
}
