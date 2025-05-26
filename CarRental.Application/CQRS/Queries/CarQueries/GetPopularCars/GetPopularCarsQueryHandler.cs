using AutoMapper;
using CarRental.Application.Dto;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces.Repositories;
using MediatR;

namespace CarRental.Application.CQRS.Queries.CarQueries.GetPopularCars;

public class GetPopularCarsQueryHandler : IRequestHandler<GetPopularCarsQuery, ICollection<CarDto>>
{
    private readonly IHomeRepository _repository;
    private readonly IMapper _mapper;
    public GetPopularCarsQueryHandler(IHomeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ICollection<CarDto>> Handle(GetPopularCarsQuery request, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var popularCars = await _repository.GetPopularCars(cancellation);

        return _mapper.Map<ICollection<CarDto>>(popularCars ?? new List<Car>());
    }
}
