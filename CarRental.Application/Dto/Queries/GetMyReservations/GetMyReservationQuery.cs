using MediatR;

namespace CarRental.Application.Dto.Queries.GetMyReservations;

public class GetMyReservationQuery : IRequest<ICollection<ReservationDto>>
{
}
