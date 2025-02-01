using CarRental.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using CarRental.Application.Dto.Queries.GetMyReservations;
using CarRental.Presentation.Models;
using AutoMapper;
using CarRental.Application.Dto.CreateReservation;

namespace CarRental.Presentation.Controllers;

[Authorize] 
public class ReservationController : Controller
{
    private readonly UserManager<IdentityUser> _userManager; 
    private readonly IMediator _mediator;
    private readonly EmailService _emailService;
    private readonly IMapper _mapper;

    public ReservationController(UserManager<IdentityUser> userManager, EmailService emailService,
        ILogger<ReservationController> logger, IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _userManager = userManager;
        _emailService = emailService;
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
        var reservation = await _context.Reservations
            .Include(r => r.Car)    // Ładujemy dane o samochodzie
            .FirstOrDefaultAsync(r => r.Id == id);  // Pobieramy rezerwację o danym ID

        var currentUserId = _userManager.GetUserId(User);

        if (reservation == null || reservation.UserId != currentUserId) // Sprawdzamy, czy rezerwacja należy do obecnego użytkownika
        {
            _logger.LogWarning($"Nie znaleziono rezerwacji o Id={id} lub brak dostępu.");
            return NotFound();
        }

        return View(reservation); // Zwracamy widok z detalami rezerwacji
    }
}
