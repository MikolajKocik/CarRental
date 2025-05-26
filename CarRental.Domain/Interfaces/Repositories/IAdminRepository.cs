using CarRental.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CarRental.Domain.Interfaces.Repositories;

public interface IAdminRepository
{
    Task<int> GetReportsCountAsync(CancellationToken cancellation);

    Task<List<Report>> GetPopularCars(CancellationToken cancellation);

    Task<IEnumerable<Reservation>> GetNotConfirmedReservationsAsync (CancellationToken cancellation);
    Task<Reservation?> GetReservationByIdAsync(int reservationId);
    Task<decimal> GetTotalIncomeAsync();
}
