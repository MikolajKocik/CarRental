using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRental.Domain.Entities;

namespace CarRental.Presentation.Controllers;


[Authorize(Roles = "Admin")]
public class CarController : Controller
{

    private readonly IMapper _mapper;

    public CarController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var cars = await _context.Cars.ToListAsync();
        return View(cars);  
    }

    [AllowAnonymous] // zarówno dla index i details użytkownicy mają dostęp
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        // Szukamy samochodu po jego ID
        var car = await _context.Cars.FindAsync(id);
        if (car == null)
        {
            return NotFound();
        }
        return View(car);
    }

    // Tylko admin ma dostęp przez atrybut nad klasą
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // obsługuje żądanie POST do utworzenia nowego samochodu
    [HttpPost]
    [ValidateAntiForgeryToken] // token dla CSRF
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

            // Dodanie nowego samochodu do bazy danych
            _context.Add(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); // Po zapisaniu przekierowujemy do listy samochodów
        }

        return View(carViewModel); // W przypadku błędu walidacji zwróć formularz
    }

    // Akcja do edytowania samochodu - get
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car == null)
        {
            return NotFound();
        }

        // Tworzymy ViewModel z danymi samochodu do edytowania
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

    // Akcja do edytowania samochodu - post
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
                // Zapisujemy zmiany w bazie danych
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Jeśli wystąpił problem z równoczesnym zapisem do bazy danych
                if (!CarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index)); // Po zapisaniu przekierowanie do listy samochodów
        }

        return View(carViewModel);
    }

    // Metoda sprawdzająca, czy samochód o danym ID istnieje w bazie
    private bool CarExists(int id)
    {
        return _context.Cars.Any(e => e.Id == id);
    }

    // Akcja do usuwania samochodu - GET
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car == null)
        {
            return NotFound();
        }

        return View(car);
    }

    // Akcja do usuwania samochodu - POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var car = await _context.Cars.FindAsync(id);

        try
        {
            if (car != null)
            {
                // Usuwamy samochód z bazy danych
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            {
                _logger.LogError($"Błąd przy usuwaniu pojazdu {ex.Message}");
            }
        }
            return RedirectToAction(nameof(Index)); // Po usunięciu przekierowanie do listy samochodów
    }
}