using MediatR;

namespace CarRental.Application.Dto.Queries.ReservationQueries.GetMyReservations;

public class GetMyReservationQuery : IRequest<ICollection<ReservationDto>>
{
}
