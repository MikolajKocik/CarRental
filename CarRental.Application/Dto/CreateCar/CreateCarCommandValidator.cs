using CarRental.Application.Services;
using CarRental.Domain.Interfaces;
using FluentValidation;

namespace CarRental.Application.Dto.CreateCar;

public class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
{
    public CreateCarCommandValidator()
    {
        RuleFor(c => c.CarDto.Brand)
            .NotEmpty().WithMessage("Brand is required");

        RuleFor(c => c.CarDto.Model)
           .NotEmpty().WithMessage("Model is required");

        RuleFor(c => c.CarDto.PricePerDay)
            .GreaterThan(0).WithMessage("Price mustn't be a value of 0");

        RuleFor(c => c.CarDto.Images)
            .Must(images => images != null && images.Any()).WithMessage("At least one image is required");

        RuleFor(c => c.CarDto.Description)
            .NotEmpty().WithMessage("Description is required");

        RuleFor(c => c.CarDto.Engine)
            .NotEmpty().WithMessage("Engine is required");

        RuleFor(c => c.CarDto.Year)
            .NotEmpty().WithMessage("Year is required.")
            .Must(CarValidationYear.IsValidYear).WithMessage("Year must be between 2010 - 2025");
    }
}

