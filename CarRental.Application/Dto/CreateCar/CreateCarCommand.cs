using MediatR;
using Microsoft.AspNetCore.Http;

namespace CarRental.Application.Dto.CreateCar
{
    public class CreateCarCommand : IRequest
    {
        public CarDto Car { get; set; } = default!;
        public IFormFile Image { get; set; } = default!;
    }
}
