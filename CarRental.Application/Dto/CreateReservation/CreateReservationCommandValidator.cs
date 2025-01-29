using CarRental.Application.Services;
using CarRental.Domain.Interfaces;
using FluentValidation;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel;


namespace CarRental.Application.Dto.CreateReservation;

public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator(IReservationRepository repository) 
    {
        RuleFor(r => r.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .Must(ReservationValidationHelpers.IsValidDate).WithMessage("Start date must not contain a time component.");

        RuleFor(r => r.EndDate)
        .NotEmpty().WithMessage("End date is required")
            .Must(ReservationValidationHelpers.IsValidDate).WithMessage("End date must not contain a time component.");
    }
}
