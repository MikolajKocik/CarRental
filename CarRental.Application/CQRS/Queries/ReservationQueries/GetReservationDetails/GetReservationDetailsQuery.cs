using CarRental.Application.Dto;
using MediatR;

namespace CarRental.Application.CQRS.Queries.ReservationQueries.GetReservationDetails
{
    public class GetReservationDetailsQuery : IRequest<ReservationDto>
    {
        public int Id { get; set; }

    }
}
