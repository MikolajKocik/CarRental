using System.ComponentModel.DataAnnotations;

namespace Wypożyczalnia_samochodów_online.Models
{
    // view model dla reservation
    public class CreateReservationViewModel
    {
        [Required(ErrorMessage = "CarId jest wymagane.")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Data rozpoczęcia jest wymagana.")] // required jako data rozpoczęcia rezerwacji
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Data zakończenia jest wymagana.")] // required ... upłynięcia rezerwacji
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
