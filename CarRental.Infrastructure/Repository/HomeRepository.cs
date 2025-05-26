using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces.Repositories;
using CarRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repository;

public class HomeRepository : IHomeRepository
{
    private readonly ApplicationDbContext _context;
    public HomeRepository(ApplicationDbContext context) 
    {
        _context = context;
    }

    public async Task<ICollection<Car>> GetPopularCars(CancellationToken cancellation)
        => await _context.Cars
        .Include(c => c.Images)
        .OrderByDescending(c => c.ReservationCount)
        .Take(3)
        .ToListAsync(cancellation);
}
