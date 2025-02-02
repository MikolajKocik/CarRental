using AutoMapper;
using CarRental.Application.Dto.ConfirmReservation;
using CarRental.Application.Dto.Queries.AdminQueries;
using CarRental.Domain.Entities;
using CarRental.Presentation.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarRental.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
  
    private readonly IWebHostEnvironment _webHostEnvironment; // Allows access to server paths (resources)
    private readonly ILogger<AdminController> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AdminController (IWebHostEnvironment webHostEnvironment,
        ILogger<AdminController> logger, IMediator mediator, IMapper mapper)
    {
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Reports()
    {
        try
        {
            var report = await _mediator.Send(new GetReportQuery());

            var viewModel = _mapper.Map<AdminReportsViewModel>(report); // reportDto -> viewModel

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching admin reports.");
            return View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken] 
    public async Task<IActionResult> ConfirmReservation()
    {
        var confirmed = await _mediator.Send(new ConfirmReservationCommand());
        return RedirectToAction(nameof(Reports));
    }

    // Metoda do tworzenia samochodu
    [HttpPost]
    [ValidateAntiForgeryToken] // token
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
            return RedirectToAction(nameof(Index)); // Po zapisaniu samochodu przekierowanie do listy samochodów
        }
        return View(car);
    }
}

