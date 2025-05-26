using CarRental.Application.Dto;
using MediatR;

namespace CarRental.Application.CQRS.Commands.Car.DeleteCar;

public class DeleteCarCommand : IRequest
{
    public int Id { get; set; }

    public CarDto Car { get; set; } = default!;
}
