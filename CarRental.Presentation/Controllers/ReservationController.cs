using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wypożyczalnia_samochodów_online.Data;
using Wypożyczalnia_samochodów_online.Models;
using Wypożyczalnia_samochodów_online.Services;

namespace Wypożyczalnia_samochodów_online.Controllers
{
    [Authorize] // Umożliwia dostęp do metod tylko dla zalogowanych użytkowników
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; // Zarządzanie użytkownikami

        private readonly EmailService _emailService;
        private readonly ILogger<ReservationController> _logger;

        public ReservationController(ApplicationDbContext context, UserManager<IdentityUser> userManager, EmailService emailService, ILogger<ReservationController> logger)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger;
        }

        // Wyświetla rezerwacje aktualnego użytkownika
        [HttpGet]
        public async Task<IActionResult> MyReservations()
        {
            var userId = _userManager.GetUserId(User);
            var reservations = await _context.Reservations
                .Where(r => r.UserId == userId) // Pobiera tylko rezerwacje tego użytkownika
                .Include(r => r.Car)    // Dołączamy samochód do rezerwacji
                .ToListAsync();    // Wykonanie zapytania do bazy

            if (!reservations.Any())
            {
                _logger.LogInformation($"Brak rezerwacji dla użytkownika: {userId}");
            }

            return View(reservations); // Zwracamy widok z rezerwacjami
        }

        // Wyświetla formularz tworzenia rezerwacji
        [HttpGet]
        public IActionResult Create(int carId)
        {
            var viewModel = new CreateReservationViewModel
            {
                CarId = carId, // Ustawiamy ID samochodu

                StartDate = DateTime.Today, // Domyślna data rozpoczęcia to dzisiejsza data
                EndDate = DateTime.Today.AddDays(1) // Domyślna data zakończenia to dzień po rozpoczęciu
            }; 
            return View(viewModel);
        }

        // Tworzy rezerwację
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReservationViewModel model)
        {

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
            if (string.IsNullOrEmpty(userId)) // Sprawdzamy, czy użytkownik jest zalogowany
            {
                _logger.LogError("No userId. User might not be logged in.");
                ModelState.AddModelError("", "Musisz być zalogowany, aby dokonać rezerwacji.");
                return RedirectToAction("Login", "Account");
            }

            var car = await _context.Cars.FindAsync(model.CarId); // Pobieramy samochód, na który robimy rezerwację
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

            if (model.EndDate <= model.StartDate)  // Sprawdzamy, czy data zakończenia jest późniejsza niż rozpoczęcia
            {
                ModelState.AddModelError("", "Data zakończenia musi być późniejsza niż data rozpoczęcia.");
                return View(model);
            }

            // Otwarcie transakcji PRZED try
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Obliczamy liczbę dni rezerwacji
                int days = (model.EndDate - model.StartDate).Days;
                if (days < 1) days = 1; // Minimalnie 1 dzień

                decimal cost = days * car.PricePerDay;  // Obliczamy koszt rezerwacji

                // Tworzymy nową rezerwację
                var reservation = new Reservation
                {
                    UserId = userId,
                    CarId = model.CarId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IsConfirmed = false, // Początkowo rezerwacja nie jest potwierdzona, potwierdza ją ADMIN
                    Car = car,
                    TotalCost = cost
                };

                // Oznacz samochód jako niedostępny
                car.IsAvailable = false;

                _context.Reservations.Add(reservation);  // Dodajemy rezerwację do bazy
                await _context.SaveChangesAsync();  // Zapisujemy zmiany

                await transaction.CommitAsync();  // Zatwierdzamy transakcję

                _logger.LogInformation($"Reservation created for CarId={car.Id}, UserId={userId}");
                return RedirectToAction(nameof(MyReservations));  // Przekierowujemy do listy rezerwacji
            }
            catch (Exception ex)
            {
                // W catch mamy wciąż dostęp do 'transaction'
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error while creating reservation.");
                return View(model);
            }
        }

        // Wyświetla szczegóły rezerwacji
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
}
