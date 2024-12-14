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
        public ReservationController(ApplicationDbContext context, UserManager<IdentityUser> userManager, EmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
        }

        // Lista rezerwacji użytkownika
        public async Task<IActionResult> MyReservations()
        {
            var userId = _userManager.GetUserId(User);
            var reservations = await _context.Reservations
                .Where(r => r.UserId == userId)
                .Include(r => r.Car)
                .ToListAsync();
            return View(reservations);
        }

        // Rezerwacja samochodu
        public IActionResult Create(int carId)
        {
            return View(new Reservation { CarId = carId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            var userId = _userManager.GetUserId(User);
            reservation.UserId = userId;

            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();

                await _emailService.SendEmailAsync(
                    User.Identity.Name,
                    "Potwierdzenie rezerwacji",
                    $"Twoja rezerwacja samochodu {reservation.Car.Brand} {reservation.Car.Model} została pomyślnie utworzona!"
                );

                return RedirectToAction(nameof(MyReservations));
            }

            return View(reservation);
        }

        // Szczegóły rezerwacji
        public async Task<IActionResult> Details(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null || reservation.UserId != _userManager.GetUserId(User))
                return NotFound();

            return View(reservation);
        }
    }
}
