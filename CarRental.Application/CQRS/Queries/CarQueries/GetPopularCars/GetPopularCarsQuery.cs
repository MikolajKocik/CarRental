using CarRental.Application.Dto;
using MediatR;

namespace CarRental.Application.CQRS.Queries.CarQueries.GetPopularCars
{
    public class GetPopularCarsQuery : IRequest<ICollection<CarDto>>
    {
    }
}
