using MediatR;

namespace CarRental.Application.Dto.EditCar;

public class EditCarCommand : IRequest
{
    public int Id { get; set; }
    public CarDto CarDto { get; set; } = default!;

}
