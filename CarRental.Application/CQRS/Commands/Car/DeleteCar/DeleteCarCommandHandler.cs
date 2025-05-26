using AutoMapper;
using CarRental.Domain.Interfaces.Repositories;
using MediatR;

namespace CarRental.Application.CQRS.Commands.Car.DeleteCar;

public class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand>
{
    private readonly ICarRepository _repository;

    public DeleteCarCommandHandler(ICarRepository repository, IMapper mapper)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteCarCommand request, CancellationToken cancellation)
    {
        var car = await _repository.GetCarByIdAsync(request.Id, cancellation);
       
        if(car != null && car.ReservationCount > 0)
        {
            car.ReservationCount--;
            await _repository.CommitAsync();
        }

            await _repository.RemoveAsync(request.Id, cancellation);

        return Unit.Value;
    }
}
