using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wypożyczalnia_samochodów_online.Models;
using Wypożyczalnia_samochodów_online.Data;
using Wypożyczalnia_samochodów_online.Models;

namespace Wypożyczalnia_samochodów_online.Controllers
{
    [Authorize(Roles = "Admin")] // Tylko admin może zarządzać samochodami
    public class CarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lista samochodów
        [AllowAnonymous] // Publicznie dostępne
        public async Task<IActionResult> Index()
        {
            var cars = await _context.Cars.ToListAsync();
            return View(cars);
        }

        // Widok dodawania samochodu
        public IActionResult Create()
        {
            return View();
        }

        // Dodawanie samochodu
        [HttpPost]
        public async Task<IActionResult> Create(Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // Edycja samochodu
        public async Task<IActionResult> Edit(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();
            return View(car);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Update(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // Usuwanie samochodu
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null) return NotFound();

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
