using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wypożyczalnia_samochodów_online.Models;
using Wypożyczalnia_samochodów_online.Data;
using Wypożyczalnia_samochodów_online.Services;

namespace Wypożyczalnia_samochodów_online.Controllers
{
    [Authorize] // Rezerwacje dostępne tylko dla zalogowanych użytkowników
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly EmailService _emailService;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(ApplicationDbContext context, UserManager<IdentityUser> userManager, EmailService emailService, ILogger<ReservationController> logger)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
        }

        // Lista rezerwacji użytkownika
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
            // Tworzymy nowy ViewModel
            var viewModel = new CreateReservationViewModel
            {
                CarId = carId,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1)
            };

            return View(viewModel);
        }

        // Rezerwacja samochodu - POST

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReservationViewModel model)
        {
            // Log na początku, by sprawdzić, czy tu w ogóle wchodzimy
            _logger.LogInformation("=== We got into POST Create! ===");

            // 1) Walidacja modelu
            if (!ModelState.IsValid)
            {
                // Wyświetlamy błędy
                foreach (var error in ModelState)
                {
                    _logger.LogWarning($"ModelState error: Key={error.Key}, " +
                        $"Errors={string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                return View(model);
            }

            // 2) Pobierz userId z kontekstu zalogowanego użytkownika
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("No userId. User might not be logged in.");
                ModelState.AddModelError("", "Musisz być zalogowany, aby dokonać rezerwacji.");
                return RedirectToAction("Login", "Account");
            }

            // 3) Sprawdź, czy jest taki Car w bazie
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

            // 4) Sprawdź logikę dat
            if (model.EndDate <= model.StartDate)
            {
                ModelState.AddModelError("", "Data zakończenia musi być późniejsza niż data rozpoczęcia.");
                return View(model);
            }

            // 5) Teraz tworzysz docelowy obiekt Reservation
            try
            {
                // (opcjonalnie transakcja, ale niekoniecznie)
                using var transaction = await _context.Database.BeginTransactionAsync();

                var reservation = new Reservation
                {
                    UserId = userId,
                    CarId = model.CarId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IsConfirmed = false,
                    Car = car  // można przypisać, jeśli chcesz mieć Car w rezerwacji od razu
                };

                // Oznacz samochód jako niedostępny
                car.IsAvailable = false;

                // Zapis do bazy
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation($"Reservation created for CarId={car.Id}, UserId={userId}");
                return RedirectToAction(nameof(MyReservations));
            }
            catch (Exception ex)
            {
                // Rollback w razie problemów
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

            if (reservation == null || reservation.UserId != _userManager.GetUserId(User))
            {
                _logger.LogWarning($"Nie znaleziono rezerwacji o Id={id} dla użytkownika.");
                return NotFound();
            }

            return View(reservation);
        }
    }
}
