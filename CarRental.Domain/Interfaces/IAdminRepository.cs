using CarRental.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CarRental.Domain.Interfaces;

public interface IAdminRepository
{
    Task<int> GetReportsCount(CancellationToken cancellation);

    Task<List<Report>> GetPopularCars(CancellationToken cancellation);

    Task<IEnumerable<Reservation>> GetNotConfirmedReservations (CancellationToken cancellation);
    Task<Reservation?> GetReservationByUserId(string? userId);
    Task<IdentityUser?> GetUserById(string? userId);
}
