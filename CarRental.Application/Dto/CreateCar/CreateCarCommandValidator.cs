using CarRental.Application.Services;
using CarRental.Domain.Interfaces;
using FluentValidation;

namespace CarRental.Application.Dto.CreateCar;

public class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
{
   public CreateCarCommandValidator(ICarRepository repository) 
    {
        RuleFor(c => c.ImageUrl)
            .NotEmpty().WithMessage("Address url is required.")
            .Must(CarValidationHelpers.IsValidUrl).WithMessage("Incorrect format url");

        RuleFor(c => c.Year)
            .NotEmpty().WithMessage("Year is required.")
            .Must(CarValidationHelpers.IsValidYear).WithMessage("Year must be between 2010 - 2025");

        RuleFor(c => c.PricePerDay)
            .NotEmpty().WithMessage("Price is required")
            .GreaterThan(0).WithMessage("Price mustn't be a value of 0");

        RuleFor(c => c.Model)
            .NotEmpty().WithMessage("Model is required");

        RuleFor(c => c.Brand)
            .NotEmpty().WithMessage("Brand is required");
    }
}

