using AutoMapper;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using MediatR;

namespace CarRental.Application.Dto.CreateCar
{
    public class CreateCarCommandHandler : IRequestHandler<CreateCarCommand>
    {
        private readonly IMapper _mapper;
        private readonly ICarRepository _repository;

        public CreateCarCommandHandler(IMapper mapper, ICarRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }
        public async Task<Unit> Handle(CreateCarCommand request, CancellationToken cancellationToken)
        {
            // mapping DTO -> car entity
            var car = _mapper.Map<Car>(request.Car);

            await _repository.Create(car);  // add to repository

            // command validation
            var validator = new CreateCarCommandValidator();
            var validationResult = validator.Validate(request);

            return Unit.Value;
        }
    }
}
