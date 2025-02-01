using CarRental.Domain.Entities;

namespace CarRental.Domain.Interfaces;

public interface IReservationRepository
{
    Task Create(Reservation reservation); // TODO

    Task<ICollection<Reservation>> GetUserReservations(string userId, CancellationToken cancellation);

}
