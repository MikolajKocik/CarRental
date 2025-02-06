using AutoMapper;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace CarRental.Application.Dto.CreateCar
{
    public class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, int>
    {
        private readonly ICarRepository _repository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public CreateCarCommandHandler(ICarRepository repository,
            IFileService fileService, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _fileService = fileService;
        }
        public async Task<int> Handle(CreateCarCommand request, CancellationToken cancellation)
        {
            // command validation
            var validator = new CreateCarCommandValidator();
            var validationResult = validator.Validate(request);

            // mapping
            var car = _mapper.Map<Car>(request.CarDto);

            // save images
            var imagePaths = request.CarDto.Images != null
                ? await _fileService.SaveFilesAsync(request.CarDto.Images, "images")
                : new List<string>();

            car.Images = imagePaths.Select(path => new CarImage { FileName = Path.GetFileName(path), Path = path}).ToList();

            await _repository.CreateAsync(car);  

            return car.Id;
        }
    }
}
