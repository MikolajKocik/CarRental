using System.ComponentModel.DataAnnotations;

namespace Wypożyczalnia_samochodów_online.Models
{
    public class CreateCarViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Marka jest wymagana")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model jest wymagany")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Cena jest wymagana")]
        [Range(1, int.MaxValue, ErrorMessage = "Cena musi być większa niż 0")]
        public decimal PricePerDay { get; set; }

        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "Obraz jest wymagany")]
        [Url(ErrorMessage = "Niepoprawny format URL.")]
        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public string Engine { get; set; }

        [Range(1900, 2100, ErrorMessage = "Wprowadź prawidłowy rok")]
        public int Year { get; set; }
    }
}
