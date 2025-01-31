using MediatR;

namespace CarRental.Application.Dto.CreateCar
{
    public class CreateCarCommand : IRequest
    {
        public CarDto Car { get; set; } = default!;
    }
}
