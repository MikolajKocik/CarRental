using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Wypożyczalnia_samochodów_online.Data;
using Wypożyczalnia_samochodów_online.Models;
using Wypożyczalnia_samochodów_online.Services;

namespace Wypożyczalnia_samochodów_online.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService; // Dodaj pole do wysyłania maila
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ApplicationDbContext context, EmailService emailService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Reports()
        {
            try
            {
                // Łączna liczba rezerwacji (wszystkich)
                var totalReservations = await _context.Reservations.CountAsync();

                // Łączny dochód (tylko potwierdzone rezerwacje)
                decimal totalIncome = await _context.Reservations
                    .Where(r => r.IsConfirmed)
                    .SumAsync(r => r.TotalCost);

                // Najpopularniejsze samochody
                var popularCars = await _context.Reservations
                    .Include(r => r.Car)
                    .GroupBy(r => new { r.Car.Id, r.Car.Brand, r.Car.Model })
                    .Select(g => new
                    {
                        g.Key.Brand,
                        g.Key.Model,
                        ReservationCount = g.Count()
                    })
                    .OrderByDescending(g => g.ReservationCount)
                    .Take(5)
                    .ToListAsync();

                // Lista niepotwierdzonych rezerwacji (opcjonalnie, jeśli chcesz je wyświetlać w raporcie)
                var notConfirmed = await _context.Reservations
                    .Include(r => r.Car)
                    .Where(r => !r.IsConfirmed)
                    .ToListAsync();

                var model = new AdminReportsViewModel
                {
                    TotalReservations = totalReservations,
                    TotalIncome = totalIncome,
                    PopularCars = popularCars.Select(pc => new CarReport
                    {
                        Brand = pc.Brand,
                        Model = pc.Model,
                        ReservationCount = pc.ReservationCount
                    }).ToList(),

                    // Dodaj, jeśli w ViewModelu masz NotConfirmedReservations:
                    NotConfirmedReservations = notConfirmed
                };

                return View(model);
            }
            catch (Exception ex)
            {
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
            var reservation = await _context.Reservations
                .Include(r => r.User) // Żeby załadować dane użytkownika
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (reservation == null)
            {
                return NotFound();
            }

            // Oznacz rezerwację jako potwierdzoną
            reservation.IsConfirmed = true;
            await _context.SaveChangesAsync();

            // === WYSŁANIE EMAILA ===
            // Jeśli IdentityUser ma Email = user@example.com, to:
            // reservation.User.Email -> docelowy adres
            // o ile user.Email nie jest null
            if (!string.IsNullOrEmpty(reservation.User?.Email))
            {
                var toEmail = reservation.User.Email;
                var subject = "Potwierdzenie rezerwacji";
                var body = $@"
                    <h2>Potwierdzenie rezerwacji</h2>
                    <p>Twoja rezerwacja o ID: {reservation.Id} została potwierdzona.</p>
                    <p>Samochód: {reservation.Car?.Brand} {reservation.Car?.Model}</p>
                    <p>Termin: {reservation.StartDate:dd.MM.yyyy} - {reservation.EndDate:dd.MM.yyyy}</p>
                    <p>Koszt: {reservation.TotalCost:C2}</p>
                    <br />
                    <p>Dziękujemy za skorzystanie z naszej wypożyczalni!</p>
                ";

                try
                {
                    await _emailService.SendEmailAsync(toEmail, subject, body);
                }
                catch (Exception ex)
                {
                    // obsługa błędu
                    Console.WriteLine($"Błąd wysyłki e-maila: {ex.Message}");
                }
            }

            return RedirectToAction(nameof(Reports));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                // Obsługuje przesyłanie pliku obrazu
                if (image != null && image.Length > 0)
                {
                    // Generowanie ścieżki do zapisu pliku
                    var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(uploadDir, fileName);

                    // Zapisywanie pliku na serwerze
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    // Przypisanie ścieżki do modelu
                    car.ImageUrl = "/Images/" + fileName;
                }

                // Dodawanie samochodu do bazy danych
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }
    }

}

