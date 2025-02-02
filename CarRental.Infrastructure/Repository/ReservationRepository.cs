using CarRental.Infrastructure.Data;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CarRental.Infrastructure.Repository;

public class ReservationRepository : IReservationRepository
{
    private readonly ApplicationDbContext _context;

    public ReservationRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager,
        ClaimsPrincipal user)
    {
        _context = context;
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
               .ToListAsync();

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellation)
        => await _context.Database.BeginTransactionAsync(cancellation);

    public async Task<Reservation?> GetReservationByIdAsync(int id, CancellationToken cancellation)
        => await _context.Reservations
            .Include(r => r.Car)
            .FirstOrDefaultAsync(r => r.Id == id, cancellation);

}
