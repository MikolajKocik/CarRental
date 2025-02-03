using AutoMapper;
using CarRental.Domain.Interfaces;
using MediatR;

namespace CarRental.Application.Dto.DeleteCar;

public class DeleteCarCommandHandler : IRequestHandler<DeleteCarCommand>
{
    private readonly ICarRepository _repository;

    public DeleteCarCommandHandler(ICarRepository repository, IMapper mapper)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteCarCommand request, CancellationToken cancellation)
    {
        var car = await _repository.GetById(request.Id, cancellation);
       
        if(car != null && car.ReservationCount > 0)
        {
            car.ReservationCount--;
            await _repository.Commit();
        }

            await _repository.Remove(request.Id, cancellation);

        return Unit.Value;
    }
}
