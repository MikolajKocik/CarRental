using System;
using System.ComponentModel.DataAnnotations; // Atrybuty walidacyjne
using System.ComponentModel.DataAnnotations.Schema; // Relacje między tabelami

namespace Wypożyczalnia_samochodów_online.Models
{
    public class Reservation
    {
        public int Id { get; set; } // Główny klucz rezerwacji

        [Required]
        public string UserId { get; set; } // Powiązanie z użytkownikiem (Identity User)

        [Required]
        public int CarId { get; set; } // Powiązanie z pojazdem

        public Car Car { get; set; } // Nawigacja do pojazdu

        [Required]
        public DateTime StartDate { get; set; } // Data rozpoczęcia wynajmu

        [Required]
        public DateTime EndDate { get; set; } // Data zakończenia wynajmu

        public bool IsConfirmed { get; set; } // Status potwierdzenia rezerwacji
    }
}
