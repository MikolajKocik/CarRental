using MediatR;

namespace CarRental.Application.Dto.CreateReservation;

public class CreateReservationCommand : IRequest
{
    public int CarId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
