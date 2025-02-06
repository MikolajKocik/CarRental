using MediatR;
using Microsoft.AspNetCore.Http;

namespace CarRental.Application.Dto.CreateCar
{
    public class CreateCarCommand : IRequest<int>
    {
        public CarDto CarDto { get; set; } = default!;
    }
}
