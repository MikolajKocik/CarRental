using CarRental.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Errors.Model;

namespace CarRental.Application.Dto.ConfirmReservation;

public class ConfirmReservationCommandHandler : IRequestHandler<ConfirmReservationCommand>
{
    private readonly ICarRepository _carRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAdminRepository _adminRepository;
    private readonly ILogger<ConfirmReservationCommandHandler> _logger;
    private readonly Services.EmailService _emailService;

    public ConfirmReservationCommandHandler(ICarRepository carRepository,
    ICurrentUserService currentUserService,
    IAdminRepository adminRepository,
    ILogger<ConfirmReservationCommandHandler> logger,
    Services.EmailService emailService)
    {
        _carRepository = carRepository;
        _currentUserService = currentUserService;
        _adminRepository = adminRepository;
        _logger = logger;
        _emailService = emailService;
    }

    public async Task<Unit> Handle(ConfirmReservationCommand request, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var userId = _currentUserService.UserId;

        var reservationByUserId = await _adminRepository.GetReservationByUserId(userId);

        if (reservationByUserId == null)
        {
            _logger.LogWarning($"Reservation not found for user {userId}");
            throw new NotFoundException("Reservation not found");
        }

        var user = await _adminRepository.GetUserById(userId);

        if (user is null)
        {
            _logger.LogWarning($"User not found for ID {userId}");
            throw new NotFoundException("User not found");
        }

        // reservation confirmed

        reservationByUserId.IsConfirmed = true;

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
                    <p>Your reservation ID: {reservationByUserId.Id} has been confirmed.</p>
                    <p>Car: {reservationByUserId.Car?.Brand} {reservationByUserId.Car?.Model}</p>
                    <p>Deadline: {reservationByUserId.StartDate:dd.MM.yyyy} - {reservationByUserId.EndDate:dd.MM.yyyy}</p>
                    <p>Cost: {reservationByUserId.TotalCost:C2}</p>
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

