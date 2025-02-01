using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace CarRental.Domain.Interfaces;

public interface IReservationRepository
{
    Task Create(Reservation reservation, CancellationToken cancellation); 

    Task<ICollection<Reservation>> GetUserReservations(string userId, CancellationToken cancellation);

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}
