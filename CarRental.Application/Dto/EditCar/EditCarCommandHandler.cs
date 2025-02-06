using AutoMapper;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using MediatR;
using SendGrid.Helpers.Errors.Model;

namespace CarRental.Application.Dto.EditCar;

public class EditCarCommandHandler : IRequestHandler<EditCarCommand>
{
    private readonly ICarRepository _repository;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;

    public EditCarCommandHandler(ICarRepository repository, IMapper mapper,
        IFileService fileService)
    {
        _repository = repository;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<Unit> Handle(EditCarCommand request, CancellationToken cancellation)
    {
        var car = await _repository.GetCarByIdAsync(request.Id, cancellation);

        if (car == null)
        {
            throw new NotFoundException($"Car with id {request.Id} not found");
        }

        // mapping

        _mapper.Map(request.CarDto, car);

        // command validation
        var validator = new EditCarCommandValidator();
        var validationResult = validator.Validate(request);

        var newImagesProvided = request.CarDto.Images != null && request.CarDto.Images.Any();

        if (newImagesProvided)
        {

            // remove old files

            await _fileService.DeleteFilesAsync(car.Images.Select(img => img.Path).ToList(), "images");

            // save new  files

            var newImagePaths = await _fileService.SaveFilesAsync(request.CarDto.Images, "images");
            car.Images = newImagePaths.Select(path => new CarImage { FileName = Path.GetFileName(path), Path = path }).ToList();

        }


        await _repository.UpdateCarAsync(car, cancellation);

        return Unit.Value;

    }
}
