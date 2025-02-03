using MediatR;

namespace CarRental.Application.Dto.ConfirmReservation;

public class ConfirmReservationCommand : IRequest
{
    public int ReservationId { get; set; }
}

