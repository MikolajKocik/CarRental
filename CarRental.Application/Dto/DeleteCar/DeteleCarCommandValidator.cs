using FluentValidation;

namespace CarRental.Application.Dto.DeleteCar;

public class DeleteCarCommandValidator : AbstractValidator<DeleteCarCommand>
{
    public DeleteCarCommandValidator()
    {
        RuleFor(x => x.Id)
       .GreaterThan(0)
       .WithMessage("Car Id must be provided and cannot be a value of 0");
    }
}
