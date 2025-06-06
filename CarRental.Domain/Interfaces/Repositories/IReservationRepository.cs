﻿using CarRental.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace CarRental.Domain.Interfaces.Repositories;

public interface IReservationRepository
{
    Task Create(Reservation reservation, CancellationToken cancellation);
    Task<ICollection<Reservation>> GetUserReservations(string userId, CancellationToken cancellation);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellation);
    Task<Reservation?> GetReservationByIdAsync(int id, CancellationToken cancellation); 
}
