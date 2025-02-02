using AutoMapper;
using CarRental.Application.Dto.UploadCarImage_Create;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using MediatR;

namespace CarRental.Application.Dto.CreateCar
{
    public class CreateCarCommandHandler : IRequestHandler<CreateCarCommand>
    {
        private readonly IMapper _mapper;
        private readonly ICarRepository _repository;
        private readonly IMediator _mediator;

        public CreateCarCommandHandler(IMapper mapper, ICarRepository repository,
            IMediator mediator)
        {
            _mapper = mapper;
            _repository = repository;
            _mediator = mediator;
        }
        public async Task<Unit> Handle(CreateCarCommand request, CancellationToken cancellation)
        {
            // mapping DTO -> car entity
            var car = _mapper.Map<Car>(request.Car);

            if (request.Image != null)
            {
                var imagePath = await _mediator.Send(new UploadCarImageCommand
                {
                    CarId = car.Id,
                    Image = request.Image
                }, cancellation);

                car.ImageUrl = imagePath;
            }

            await _repository.Create(car);  // add to repository

            // command validation
            var validator = new CreateCarCommandValidator();
            var validationResult = validator.Validate(request);

            return Unit.Value;
        }
    }
}
