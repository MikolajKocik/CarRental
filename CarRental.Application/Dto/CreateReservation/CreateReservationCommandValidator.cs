using FluentValidation;

namespace CarRental.Application.Dto.CreateReservation;

public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator() 
    {
        RuleFor(r => r.StartDate)
            .LessThan(r => r.EndDate)
            .WithMessage("Start date must be earlier than End date.");

        RuleFor(r => r.EndDate)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Start date cannot be in the past.");
        
    }
}
