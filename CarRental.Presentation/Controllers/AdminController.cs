using AutoMapper;
using CarRental.Application.Dto.Queries.AdminQueries;
using CarRental.Application.Services;
using CarRental.Domain.Entities;
using CarRental.Presentation.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CarRental.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly EmailService _emailService; 
    private readonly IWebHostEnvironment _webHostEnvironment; // Allows access to server paths (resources)
    private readonly ILogger<AdminController> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AdminController (EmailService emailService, IWebHostEnvironment webHostEnvironment,
        ILogger<AdminController> logger, IMediator mediator, IMapper mapper)
    {
        _emailService = emailService;
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

    // Metoda do potwierdzenia rezerwacji
    [HttpPost]
    [ValidateAntiForgeryToken] // token
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
        // UWAGA: email musi być prawdziwy, żeby wysłać potwierdzenie
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
                _logger.LogInformation($"Próba wysłania e-maila do {toEmail}");
                await _emailService.SendEmailAsync(toEmail, subject, body);
                _logger.LogInformation($"E-mail do {toEmail} został wysłany.");
            }
            catch (Exception ex)
            {
                // obsługa błędu
                _logger.LogError($"Błąd wysyłki e-maila: {ex.Message}");
            }
        }

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

