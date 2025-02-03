using CarRental.Domain.Entities;

namespace CarRental.Presentation.Models
{
    public class MyReservationsViewModel
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsConfirmed { get; set; }
        public decimal TotalCost { get; set; }

        public string UserId { get; set; } = default!;

        public int CarId { get; set; }
        public Car Car { get; set; } = default!;
    }
}
