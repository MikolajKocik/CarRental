using MediatR;

namespace CarRental.Application.Dto.Queries.CarDetails
{
    public class CarDetailsQuery : IRequest<CarDto>
    {
        public int Id { get; }

        public CarDetailsQuery(int id)
        {
            Id = id;
        }
    }
}
