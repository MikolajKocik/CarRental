using MediatR;

namespace CarRental.Application.Dto.DeleteCar;

public class DeleteCarCommand : IRequest
{
    public int Id { get; set; }

    public CarDto Car { get; set; } = default!;
}
