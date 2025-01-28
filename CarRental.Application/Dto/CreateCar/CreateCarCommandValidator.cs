using CarRental.Application.Services;
using CarRental.Domain.Interfaces;
using FluentValidation;

namespace CarRental.Application.Dto.CreateCar
{
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
        }
    }
}

