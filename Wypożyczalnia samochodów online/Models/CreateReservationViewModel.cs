using System.ComponentModel.DataAnnotations;

namespace Wypożyczalnia_samochodów_online.Models
{
    public class CreateReservationViewModel
    {
        [Required(ErrorMessage = "CarId jest wymagane.")]
        public int CarId { get; set; }

        [Required(ErrorMessage = "Data rozpoczęcia jest wymagana.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Data zakończenia jest wymagana.")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
