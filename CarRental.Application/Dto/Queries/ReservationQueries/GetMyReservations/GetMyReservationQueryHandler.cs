using AutoMapper;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using MediatR;

namespace CarRental.Application.Dto.Queries.ReservationQueries.GetMyReservations;

public class GetMyReservationQueryHandler : IRequestHandler<GetMyReservationQuery, ICollection<ReservationDto>>
{
    // Uses ICurrentUserService to retrieve the user ID directly from the HTTP context.
    private readonly ICurrentUserService _currentUserService;
    private readonly IReservationRepository _repository;
    private readonly IMapper _mapper;
    public GetMyReservationQueryHandler(IReservationRepository repository, IMapper mapper,
        ICurrentUserService currentUserService)
    {
        _repository = repository;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }
    public async Task<ICollection<ReservationDto>> Handle(GetMyReservationQuery request, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        // We get the current user ID using ICurrentUserService.
        var userId = _currentUserService.UserId;

        var myReservations = await _repository.GetUserReservations(userId!, cancellation);

        return _mapper.Map<ICollection<ReservationDto>>(myReservations ?? new List<Reservation>());

    }
}
