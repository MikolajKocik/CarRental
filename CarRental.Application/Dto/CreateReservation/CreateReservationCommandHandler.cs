using AutoMapper;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Errors.Model;
using System.ComponentModel.DataAnnotations;


namespace CarRental.Application.Dto.CreateReservation;

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<CreateReservationCommandHandler> _logger;
    private readonly ICarRepository _carRepository;

    public CreateReservationCommandHandler(
        IReservationRepository reservationRepository,
        ICarRepository carRepository,
        ICurrentUserService currentUserService,
        ILogger<CreateReservationCommandHandler> logger,
        IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _carRepository = carRepository;
        _currentUserService = currentUserService;
        _logger = logger;
    }
    public async Task<Unit> Handle(CreateReservationCommand request, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var userId = _currentUserService.UserId;

        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogError("No userId. User might not be logged in.");
            throw new UnauthorizedAccessException("You must be logged in to make a reservation.");
        }

        if (request.EndDate <= request.StartDate)
        {
            throw new ValidationException("The end date must be later than the start date.");
        }

        var car = await _carRepository.GetById(request.CarId, cancellation);
        if (car == null)
        {
            throw new NotFoundException($"The selected car (CarId={request.CarId}) does not exist.");
        }
        if (!car.IsAvailable)
        {
            throw new ValidationException("The selected car is unavailable.");
        }
        
        int days = (request.EndDate - request.StartDate).Days;
        if (days < 1) days = 1;
        decimal cost = days * car.PricePerDay;

        using (var transaction = await _reservationRepository.BeginTransactionAsync(cancellation))
        {
            try
            {
                // Create reservation
                var reservation = new Reservation
                {
                    UserId = userId,
                    CarId = request.CarId,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    IsConfirmed = false, // reservation is not confirmed at the beginning
                    TotalCost = cost,
                    Car = car
                };

                // Mark the car as not available
                car.IsAvailable = false;

              
                await _reservationRepository.Create(reservation, cancellation);

                
                await transaction.CommitAsync(cancellation);
            }
            catch (Exception ex)
            {
                // roll back transaction if not succeed
                await transaction.RollbackAsync(cancellation);
                _logger.LogError(ex, "Error while creating reservation.");
                throw;
            }
        }

        _logger.LogInformation($"Reservation created for CarId = {request.CarId}, UserId = {userId}");
        return Unit.Value;
    }
}
