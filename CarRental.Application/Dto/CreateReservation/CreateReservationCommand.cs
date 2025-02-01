using MediatR;

namespace CarRental.Application.Dto.CreateReservation;

public class CreateReservationCommand : IRequest
{
    public ReservationDto Reservation { get; set; } = new ReservationDto();
}
