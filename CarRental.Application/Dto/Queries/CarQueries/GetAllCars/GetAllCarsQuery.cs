using MediatR;

namespace CarRental.Application.Dto.Queries.CarQueries.GetAllCars;

public class GetAllCarsQuery : IRequest<ICollection<CarDto>>
{
}
