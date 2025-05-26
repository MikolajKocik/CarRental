using MediatR;

namespace CarRental.Application.CQRS.Commands.Reservation.ConfirmReservation;

public class ConfirmReservationCommand : IRequest
{
    public int ReservationId { get; set; }
}

