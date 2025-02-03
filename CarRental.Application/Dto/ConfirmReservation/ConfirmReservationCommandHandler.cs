using CarRental.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Errors.Model;

namespace CarRental.Application.Dto.ConfirmReservation;

public class ConfirmReservationCommandHandler : IRequestHandler<ConfirmReservationCommand>
{
    private readonly ICarRepository _carRepository;
    private readonly IAdminRepository _adminRepository;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<ConfirmReservationCommandHandler> _logger;
    private readonly Services.EmailService _emailService;

    public ConfirmReservationCommandHandler(ICarRepository carRepository,
    IAdminRepository adminRepository,
    UserManager<IdentityUser> userManager,
    ILogger<ConfirmReservationCommandHandler> logger,
    Services.EmailService emailService)
    {
        _carRepository = carRepository;
        _adminRepository = adminRepository;
        _logger = logger;
        _emailService = emailService;
        _userManager = userManager;
    }

    public async Task<Unit> Handle(ConfirmReservationCommand request, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var reservation = await _adminRepository.GetReservationByIdAsync(request.ReservationId);

        if (reservation == null)
        {
            _logger.LogWarning($"Reservation with ID {request.ReservationId} not found.");
            throw new NotFoundException("Reservation not found");
        }

        var user = await _userManager.FindByIdAsync(reservation.UserId);

        if (user == null)
        {
            _logger.LogWarning($"User with ID {reservation.UserId} not found.");
            throw new NotFoundException("User not found");
        }

        reservation.IsConfirmed = true;

        await _carRepository.Commit();

        // === SENDING EMAIL ===
        // If IdentityUser has Email = user@example.com, then:
        // reservation.User.Email -> target address
        // if user.Email is not null
        // NOTE: email must be real to send confirmation

        if (!string.IsNullOrEmpty(user?.Email))
        {
            var toEmail = user.Email;
            var subject = "Confirmation of reservation";
            var body = $@"
                    <h2>Confirmation of reservation</h2>
                    <p>Your reservation ID: {reservation.Id} has been confirmed.</p>
                    <p>Car: {reservation.Car?.Brand} {reservation.Car?.Model}</p>
                    <p>Deadline: {reservation.StartDate:dd.MM.yyyy} - {reservation.EndDate:dd.MM.yyyy}</p>
                    <p>Cost: {reservation.TotalCost:C2}</p>
                    <br />
                    <p>Thank you for using our rental service!</p>
                ";

            try
            {
                _logger.LogInformation($"Attempt of sending e-mail to {toEmail}");
                await _emailService.SendEmailAsync(toEmail, subject, body);
                _logger.LogInformation($"E-mail to {toEmail} was send.");
            }
            catch (Exception ex)
            {
                // email sending error
                _logger.LogError($"Error while sending e-mail: {ex.Message}");
            }
        }

        return Unit.Value;
    }
}

