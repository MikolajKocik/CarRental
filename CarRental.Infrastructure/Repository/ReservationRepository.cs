﻿using CarRental.Infrastructure.Data;
using CarRental.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using CarRental.Domain.Interfaces.Repositories;

namespace CarRental.Infrastructure.Repository;

public class ReservationRepository : IReservationRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ReservationRepository> _logger;

    public ReservationRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager,
        ILogger<ReservationRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Create(Reservation reservation, CancellationToken cancellation)
    {
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync(cancellation);
    }

    public async Task<ICollection<Reservation>> GetUserReservations(string userId, CancellationToken cancellation)
        => await _context.Reservations
               .Where(r => r.UserId == userId)
               .Include(r => r.Car)
               .ToListAsync(cancellation);

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellation)
        => await _context.Database.BeginTransactionAsync(cancellation);

    public async Task<Reservation?> GetReservationByIdAsync(int id, CancellationToken cancellation)
    {
        _logger.LogInformation($"Executing SQL query for UserId: {id}");

        return await _context.Reservations
            .Include(r => r.Car)
            .FirstOrDefaultAsync(r => r.Id == id, cancellation);
    }
}
