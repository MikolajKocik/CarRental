using CarRental.Application.Dto;
using MediatR;

namespace CarRental.Application.CQRS.Queries.CarQueries.GetCarDetails
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
