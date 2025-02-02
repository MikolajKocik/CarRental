using CarRental.Domain.Entities;

namespace CarRental.Domain.Interfaces;

public interface IAdminRepository
{
    Task<int> GetReportsCount(CancellationToken cancellation);

    Task<List<Report>> GetPopularCars(CancellationToken cancellation);

    Task<IEnumerable<Reservation>> GetNotConfirmedReservations (CancellationToken cancellation);

}
