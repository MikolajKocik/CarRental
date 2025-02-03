using MediatR;

namespace CarRental.Application.Dto.Queries.CarQueries.GetCarDetails
{
    public class CarDetailsQuery : IRequest<CarDto>
    {
        public int Id { get; set; }

        public CarDetailsQuery(int id )
        {
            Id = id;
        }
    }
}
