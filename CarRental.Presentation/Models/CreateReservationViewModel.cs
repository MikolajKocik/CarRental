using System.ComponentModel.DataAnnotations;

namespace CarRental.Presentation.Models
{
    public class CreateReservationViewModel
    {
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
