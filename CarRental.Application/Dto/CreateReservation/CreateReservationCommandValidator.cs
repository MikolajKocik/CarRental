using FluentValidation;

namespace CarRental.Application.Dto.CreateReservation;

public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator() 
    {
        RuleFor(r => r.Reservation.StartDate)
            .LessThan(r => r.Reservation.EndDate)
            .WithMessage("Start date must be earlier than End date.");

        RuleFor(r => r.Reservation.EndDate)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Start date cannot be in the past.");
        
    }
}
