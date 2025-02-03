using FluentValidation;

namespace CarRental.Application.Dto.DeleteCar;

public class DeleteCarCommandValidator : AbstractValidator<DeleteCarCommand>
{
    public DeleteCarCommandValidator()
    {      
    }
}
