using AutoMapper;
using CarRental.Domain.Interfaces;
using MediatR;

namespace CarRental.Application.Dto.Queries;

public class GetAllCarsQueryHandler : IRequestHandler<GetAllCarsQuery, ICollection<CarDto>>
{
    private readonly ICarRepository _carRepository;
    private readonly IMapper _mapper;

    public GetAllCarsQueryHandler(ICarRepository carRepository, IMapper mapper)
    {
        _carRepository = carRepository;
        _mapper = mapper;
    }

    public async Task<ICollection<CarDto>> Handle(GetAllCarsQuery request, CancellationToken cancellation)
    {
        var cars = await _carRepository.GetAll();
        var dtos = _mapper.Map<ICollection<CarDto>>(cars);

        return dtos;
    }
}
