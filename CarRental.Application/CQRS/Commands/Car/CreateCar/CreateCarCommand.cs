using CarRental.Application.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CarRental.Application.CQRS.Commands.Car.CreateCar
{
    public class CreateCarCommand : IRequest<int>
    {
        public CarDto CarDto { get; set; } = default!;
    }
}
