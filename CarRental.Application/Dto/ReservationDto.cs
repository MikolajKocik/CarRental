using CarRental.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace CarRental.Application.Dto
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsConfirmed { get; set; }
        public decimal TotalCost { get; set; }

        public string UserName { get; set; } = default!;  // Identity user

        public int CarId { get; set; }
        public CarDto Car { get; set; } = default!;
    }
}
