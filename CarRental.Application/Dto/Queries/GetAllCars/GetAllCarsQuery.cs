using MediatR;

namespace CarRental.Application.Dto.Queries.GetAllCars;

public class GetAllCarsQuery : IRequest<ICollection<CarDto>>
{
}
