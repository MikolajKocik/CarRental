using CarRental.Application.Dto;
using MediatR;

namespace CarRental.Application.CQRS.Queries.ReservationQueries.GetMyReservations;

public class GetMyReservationQuery : IRequest<ICollection<ReservationDto>>
{
}
