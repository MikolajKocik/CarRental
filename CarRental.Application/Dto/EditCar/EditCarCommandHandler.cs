using AutoMapper;
using CarRental.Domain.Interfaces;
using MediatR;
using SendGrid.Helpers.Errors.Model;

namespace CarRental.Application.Dto.EditCar;

public class EditCarCommandHandler : IRequestHandler<EditCarCommand>
{
    private readonly ICarRepository _repository;
    private readonly IMapper _mapper;

    public EditCarCommandHandler(ICarRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(EditCarCommand request, CancellationToken cancellation)
    {
        var car = await _repository.GetById(request.Id, cancellation);

        if (car == null)
        {
            throw new NotFoundException($"Car with id {request.Id} not found");
        }

        // command validation
        var validator = new EditCarCommandValidator();
        var validationResult = validator.Validate(request);

        // edit data
        car.Brand = request.Car.Brand;
        car.Model = request.Car.Model;
        car.PricePerDay = request.Car.PricePerDay;
        car.IsAvailable = request.Car.IsAvailable;
        car.ImageUrl = request.Car.ImageUrl;
        car.Description = request.Car.Description;
        car.Engine = request.Car.Engine;
        car.Year = request.Car.Year;

        await _repository.Commit();

        return Unit.Value;

    }
}
