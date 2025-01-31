using MediatR;

namespace CarRental.Application.Dto.Queries.CarDetails
{
    public class CarDetailsQuery : IRequest<CarDto>
    {
        public int Id { get; set; }

        public CarDetailsQuery(int id)
        {
            Id = id;
        }
    }
}
