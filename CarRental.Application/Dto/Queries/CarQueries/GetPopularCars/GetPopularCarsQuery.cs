using MediatR;

namespace CarRental.Application.Dto.Queries.CarQueries.GetPopularCars
{
    public class GetPopularCarsQuery : IRequest<ICollection<CarDto>>
    {
    }
}
