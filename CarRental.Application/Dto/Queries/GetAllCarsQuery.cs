using MediatR;

namespace CarRental.Application.Dto.Queries;

public class GetAllCarsQuery : IRequest<ICollection<CarDto>>
{
}
