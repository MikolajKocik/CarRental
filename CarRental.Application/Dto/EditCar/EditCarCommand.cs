using MediatR;

namespace CarRental.Application.Dto.EditCar;

public class EditCarCommand : IRequest
{
    public int Id { get; set; }
    public CarDto Car { get; set; } = default!;

}
