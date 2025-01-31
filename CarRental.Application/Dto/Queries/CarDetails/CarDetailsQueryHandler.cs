using AutoMapper;
using CarRental.Domain.Interfaces;
using MediatR;
using SendGrid.Helpers.Errors.Model;

namespace CarRental.Application.Dto.Queries.CarDetails
{
    public class CarDetailsQueryHandler : IRequestHandler<CarDetailsQuery, CarDto>
    {
        private readonly ICarRepository _repository;
        private readonly IMapper _mapper;

        public CarDetailsQueryHandler(ICarRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CarDto> Handle(CarDetailsQuery request, CancellationToken cancellationToken)
        {

            cancellationToken.ThrowIfCancellationRequested();

            var car = await _repository.GetById(request.Id, cancellationToken);

            if (car == null)
            {
                throw new NotFoundException($"Car with id {request.Id} not found");
            }

            var dtos = _mapper.Map<CarDto>(car);

            return dtos;
        }
    }
}
