using CarRental.Application.Dto;
using MediatR;

namespace CarRental.Application.CQRS.Commands.Car.EditCar;

public class EditCarCommand : IRequest
{
    public int Id { get; set; }
    public CarDto CarDto { get; set; } = default!;

}
