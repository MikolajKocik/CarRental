using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRental.Presentation.Models;
using CarRental.Application.Dto;
using CarRental.Application.Dto.CreateCar;
using MediatR;
using CarRental.Application.Dto.Queries;

namespace CarRental.Presentation.Controllers;


[Authorize(Roles = "Admin")]
public class CarController : Controller
{

    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CarController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var cars = await _mediator.Send(new GetAllCarsQuery());
        return View(cars);  
    }

    [AllowAnonymous] 
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        
        var car = await _context.Cars.FindAsync(id);
        if (car == null)
        {
            return NotFound();
        }
        return View(car);
    }

    [HttpGet] // Admin
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken] // Admin
    public async Task<IActionResult> Create(CreateCarViewModel carViewModel)
    {
        if (ModelState.IsValid)
        {
            // mapping view model -> DTO
            var carDto = _mapper.Map<CarDto>(carViewModel);

            // now ViewModel = validation of command
            var command = new CreateCarCommand { Car =  carDto };

            await _mediator.Send(command);
            return RedirectToAction("Index");
        }

        return View(carViewModel); 
    }


    [HttpGet]
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