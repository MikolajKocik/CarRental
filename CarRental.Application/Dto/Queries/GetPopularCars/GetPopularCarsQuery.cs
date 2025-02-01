using MediatR;

namespace CarRental.Application.Dto.Queries.GetPopularCars
{
    public class GetPopularCarsQuery : IRequest<ICollection<CarDto>>
    {
    }
}
