using MediatR;

namespace CarRental.Application.Dto.Queries.ReservationQueries.GetReservationDetails
{
    public class GetReservationDetailsQuery : IRequest<ReservationDto>
    {
        public int Id { get; set; }

    }
}
