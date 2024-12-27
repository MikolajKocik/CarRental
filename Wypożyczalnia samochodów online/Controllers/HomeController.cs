using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wypożyczalnia_samochodów_online.Data;
using Wypożyczalnia_samochodów_online.Models;

namespace Wypożyczalnia_samochodów_online.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; // Logger do logowania informacji o aplikacji
        private readonly ApplicationDbContext _context; // Dodanie kontekstu bazy danych

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger; // Inicjalizacja loggera
            _context = context; // Inicjalizacja kontekstu bazy
        }

        // Strona główna
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Pobieramy trzy najnowsze samochody z bazy danych
            var cars = await _context.Cars.OrderBy(c => c.Id).Take(3).ToListAsync();
            return View(cars); // Zwracamy widok z tymi samochodami
        }

        // Strona prywatności
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();  // Zwracamy widok strony prywatności
        }

        // Obsługuje błędy aplikacji (np. gdy nie znajdzie jakiegoś zasobu)
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
