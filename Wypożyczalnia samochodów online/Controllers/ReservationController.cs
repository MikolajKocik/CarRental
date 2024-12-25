using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wypożyczalnia_samochodów_online.Data;
using Wypożyczalnia_samochodów_online.Models;
using Wypożyczalnia_samochodów_online.Services;

namespace Wypożyczalnia_samochodów_online.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly EmailService _emailService;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            EmailService emailService,
            ILogger<ReservationController> logger)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<IActionResult> MyReservations()
        {
            var userId = _userManager.GetUserId(User);
            var reservations = await _context.Reservations
                .Where(r => r.UserId == userId)
                .Include(r => r.Car)
                .ToListAsync();

            if (!reservations.Any())
            {
                _logger.LogInformation($"Brak rezerwacji dla użytkownika: {userId}");
            }

            return View(reservations);
        }

        // GET: Reservation/Create
        public IActionResult Create(int carId)
        {
            var viewModel = new CreateReservationViewModel
            {
                CarId = carId,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1)
            };
            return View(viewModel);
        }

        // POST: Reservation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReservationViewModel model)
        {
            _logger.LogInformation("=== We got into POST Create! ===");

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState)
                {
                    _logger.LogWarning($"ModelState error: Key={error.Key}, " +
                        $"Errors={string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return View(model);
            }

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("No userId. User might not be logged in.");
                ModelState.AddModelError("", "Musisz być zalogowany, aby dokonać rezerwacji.");
                return RedirectToAction("Login", "Account");
            }

            var car = await _context.Cars.FindAsync(model.CarId);
            if (car == null)
            {
                ModelState.AddModelError("", "Wybrany samochód nie istnieje.");
                return View(model);
            }
            else if (!car.IsAvailable)
            {
                ModelState.AddModelError("", "Wybrany samochód jest niedostępny.");
                return View(model);
            }

            if (model.EndDate <= model.StartDate)
            {
                ModelState.AddModelError("", "Data zakończenia musi być późniejsza niż data rozpoczęcia.");
                return View(model);
            }

            // Otwarcie transakcji PRZED try
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                int days = (model.EndDate - model.StartDate).Days;
                if (days < 1) days = 1;

                decimal cost = days * car.PricePerDay;

                var reservation = new Reservation
                {
                    UserId = userId,
                    CarId = model.CarId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IsConfirmed = false,
                    Car = car,
                    TotalCost = cost
                };

                // Oznacz samochód jako niedostępny
                car.IsAvailable = false;

                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                _logger.LogInformation($"Reservation created for CarId={car.Id}, UserId={userId}");
                return RedirectToAction(nameof(MyReservations));
            }
            catch (Exception ex)
            {
                // W catch mamy wciąż dostęp do 'transaction'
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error while creating reservation.");
                return View(model);
            }
        }

        // Szczegóły rezerwacji
        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == id);

            var currentUserId = _userManager.GetUserId(User);

            if (reservation == null || reservation.UserId != currentUserId)
            {
                _logger.LogWarning($"Nie znaleziono rezerwacji o Id={id} lub brak dostępu.");
                return NotFound();
            }

            return View(reservation);
        }
    }
}
