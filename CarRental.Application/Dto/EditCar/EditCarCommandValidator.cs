using CarRental.Application.Services;
using FluentValidation;

namespace CarRental.Application.Dto.EditCar;

public class EditCarCommandValidator : AbstractValidator<EditCarCommand>
{
    public EditCarCommandValidator()
    {
        RuleFor(c => c.Car.Brand)
            .NotEmpty().WithMessage("Brand is required");

        RuleFor(c => c.Car.Model)
           .NotEmpty().WithMessage("Model is required");

        RuleFor(c => c.Car.PricePerDay)
            .GreaterThan(0).WithMessage("Price mustn't be a value of 0");

        RuleFor(c => c.Car.ImageUrl)
            .NotEmpty().WithMessage("Address url is required")
            .Must(CarValidationHelpers.IsValidUrl).WithMessage("Incorrect format url");

        RuleFor(c => c.Car.Description)
            .NotEmpty().WithMessage("Description is required");

        RuleFor(c => c.Car.Engine)
            .NotEmpty().WithMessage("Engine is required");

        RuleFor(c => c.Car.Year)
            .NotEmpty().WithMessage("Year is required.")
            .Must(CarValidationHelpers.IsValidYear).WithMessage("Year must be between 2010 - 2025");
    }
}
