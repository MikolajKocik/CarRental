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

        _mapper.Map(request.Car, car);

        await _repository.Commit();

        return Unit.Value;  

    }
}
