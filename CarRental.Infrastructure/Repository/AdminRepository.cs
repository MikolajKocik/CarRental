using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using CarRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Report>> GetPopularCars(CancellationToken cancellation)
            => await _context.Reservations
            .Include(r => r.Car)
             .GroupBy(r => new { r.Car.Id, r.Car.Brand, r.Car.Model })
                .Select(g => new Report
                {
                    CarName = $"{g.Key.Brand} {g.Key.Model}",
                    ReservationsCount = g.Count(),
                    TotalIncome = g.Sum(r => r.TotalCost)
                })
                .OrderByDescending(g => g.ReservationsCount)
                .Take(3)
                .ToListAsync(cancellation);

        public async Task<int> GetReportsCount(CancellationToken cancellation)
            => await _context.Reservations.CountAsync(cancellation);

        public async Task<IEnumerable<Reservation>> GetNotConfirmedReservations(CancellationToken cancellation)
            => await _context.Reservations
            .Include(r => r.Car)
            .Where(r => !r.IsConfirmed)
            .ToListAsync(cancellation);


    }
}
