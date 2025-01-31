using AutoMapper;
using CarRental.Domain.Interfaces;
using MediatR;

namespace CarRental.Application.Dto.Queries.GetAllCars;

public class GetAllCarsQueryHandler : IRequestHandler<GetAllCarsQuery, ICollection<CarDto>>
{
    private readonly ICarRepository _repository;
    private readonly IMapper _mapper;

    public GetAllCarsQueryHandler(ICarRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ICollection<CarDto>> Handle(GetAllCarsQuery request, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var cars = await _repository.GetAll(cancellation);
        var dtos = _mapper.Map<ICollection<CarDto>>(cars);

        return dtos;
    }
}
