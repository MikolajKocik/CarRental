using CarRental.Application.Dto;
using MediatR;

namespace CarRental.Application.CQRS.Queries.CarQueries.GetAllCars;

public class GetAllCarsQuery : IRequest<ICollection<CarDto>>
{
}
