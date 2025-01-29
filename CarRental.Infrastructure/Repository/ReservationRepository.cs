using CarRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;

namespace CarRental.Infrastructure.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task Create(Reservation reservation)
        {
            throw new NotImplementedException(); // TODO 
        }

        public async Task<Reservation?> GetByIdAsync(int reservationId)
            => await _context.Reservations
            .Include(r => r.Car)
            .FirstOrDefaultAsync(r => r.Id == reservationId);
    }
}
