using System;
using System.ComponentModel.DataAnnotations; // Atrybuty walidacyjne
using System.ComponentModel.DataAnnotations.Schema; // Relacje między tabelami
using Microsoft.AspNetCore.Identity;

namespace Wypożyczalnia_samochodów_online.Models
{
    public class Reservation
    {
        public int Id { get; set; } // Główny klucz rezerwacji

        [Required]
        public string UserId { get; set; } // Powiązanie z użytkownikiem (Identity User)

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }

        [Required]
        public int CarId { get; set; } // Powiązanie z pojazdem

        [ForeignKey("CarId")]
        public Car Car { get; set; } // Nawigacja do pojazdu

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data rozpoczęcia")]
        public DateTime StartDate { get; set; } // Data rozpoczęcia wynajmu

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data zakończenia")]
        public DateTime EndDate { get; set; } // Data zakończenia wynajmu

        public bool IsConfirmed { get; set; } // Status potwierdzenia rezerwacji
    }
}
