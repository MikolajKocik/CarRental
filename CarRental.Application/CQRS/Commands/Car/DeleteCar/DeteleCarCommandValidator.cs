using FluentValidation;

namespace CarRental.Application.CQRS.Commands.Car.DeleteCar;

public class DeleteCarCommandValidator : AbstractValidator<DeleteCarCommand>
{
    public DeleteCarCommandValidator()
    {      
    }
}
