using AutoMapper;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using MediatR;

namespace CarRental.Application.Dto.CreateCar
{
    public class CreateCarCommandHandler : IRequestHandler<CreateCarCommand>
    {
        private readonly IMapper _mapper;
        private readonly ICarRepository _carRepository;

        public CreateCarCommandHandler(IMapper mapper, ICarRepository carRepository)
        {
            _mapper = mapper;
            _carRepository = carRepository;
        }
        public async Task<Unit> Handle(CreateCarCommand command, CancellationToken cancellationToken)
        {
            // mapping DTO -> car entity
            var car = _mapper.Map<Car>(command.Car);

            await _carRepository.Create(car);  // add to repository

            // command validation
            var validator = new CreateCarCommandValidator(_carRepository);
            var validationResult = validator.Validate(command);

            return Unit.Value;
        }
    }
}
