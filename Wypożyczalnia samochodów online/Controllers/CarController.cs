using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wypożyczalnia_samochodów_online.Data;
using Wypożyczalnia_samochodów_online.Models;

namespace Wypożyczalnia_samochodów_online.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CarController> _logger;

        public CarController(ApplicationDbContext context, ILogger<CarController> logger)
        {
            _context = context;
            _logger= logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var cars = await _context.Cars.ToListAsync();
            return View(cars);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        // Admin only: Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCarViewModel carViewModel)
        {
            if (ModelState.IsValid)
            {
                var car = new Car
                {
                    Brand = carViewModel.Brand,
                    Model = carViewModel.Model,
                    PricePerDay = carViewModel.PricePerDay,
                    IsAvailable = carViewModel.IsAvailable,
                    ImageUrl = carViewModel.ImageUrl,
                    Description = carViewModel.Description,
                    Engine = carViewModel.Engine,
                    Year = carViewModel.Year
                };

                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Po zapisaniu przekierowujemy do listy samochodów
            }

            return View(carViewModel); // W przypadku błędu walidacji zwróć formularz
        }

        public async Task<IActionResult> Edit(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            var carViewModel = new CreateCarViewModel
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                PricePerDay = car.PricePerDay,
                IsAvailable = car.IsAvailable,
                ImageUrl = car.ImageUrl,
                Description = car.Description,
                Engine = car.Engine,
                Year = car.Year
            };

            return View(carViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateCarViewModel carViewModel)
        {
            if (id != carViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var car = await _context.Cars.FindAsync(id);
                if (car == null)
                {
                    return NotFound();
                }

                // Mapowanie danych z ViewModelu do modelu Car
                car.Brand = carViewModel.Brand;
                car.Model = carViewModel.Model;
                car.PricePerDay = carViewModel.PricePerDay;
                car.IsAvailable = carViewModel.IsAvailable;
                car.ImageUrl = carViewModel.ImageUrl;
                car.Description = carViewModel.Description;
                car.Engine = carViewModel.Engine;
                car.Year = carViewModel.Year;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(carViewModel);
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
