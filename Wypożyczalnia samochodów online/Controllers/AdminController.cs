using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Wypożyczalnia_samochodów_online.Data;
using Wypożyczalnia_samochodów_online.Models;

namespace Wypożyczalnia_samochodów_online.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Reports()
        {
            try
            {
                // Łączna liczba rezerwacji
                var totalReservations = await _context.Reservations.CountAsync();

                // Łączny dochód (na podstawie rezerwacji)
                var totalIncome = await _context.Reservations
                    .Where(r => r.IsConfirmed)
                    .Select(r => new
                    {
                        Days = EF.Functions.DateDiffDay(r.StartDate, r.EndDate),
                        PricePerDay = r.Car.PricePerDay
                    })
                    .SumAsync(r => r.Days * r.PricePerDay);

                // Najpopularniejsze samochody (grupowanie po liczbie rezerwacji)
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

                // Stwórz model widoku
                var model = new AdminReportsViewModel
                {
                    TotalReservations = totalReservations,
                    TotalIncome = totalIncome,
                    PopularCars = popularCars
                        .Select(pc => new CarReport
                        {
                            Brand = pc.Brand,
                            Model = pc.Model,
                            ReservationCount = pc.ReservationCount
                        })
                        .ToList()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                // Logowanie błędu (możesz użyć loggera)
                // Przykład: _logger.LogError(ex, "Error in Reports action");

                // Przekierowanie do strony błędu lub wyświetlenie komunikatu
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

    }
}
