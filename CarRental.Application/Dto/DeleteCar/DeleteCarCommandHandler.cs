using AutoMapper;
using CarRental.Domain.Interfaces;
using MediatR;
using SendGrid.Helpers.Errors.Model;

namespace CarRental.Application.Dto.DeleteCar;

public class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand>
{
    private readonly ICarRepository _repository;
    private readonly IMapper _mapper;

    public DeleteCarCommandHandler(ICarRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(DeleteCarCommand request, CancellationToken cancellation)
    {
        var car = await _repository.GetById(request.Id, cancellation);

        var validator = new DeleteCarCommandValidator();

        var validatorResult = validator.Validate(request);

        if (validatorResult.IsValid)
        {
            throw new Exception($"Car id {request.Id} was no found");
        }

        await _repository.Remove(request.Id, cancellation);

        return Unit.Value;
    }
}
