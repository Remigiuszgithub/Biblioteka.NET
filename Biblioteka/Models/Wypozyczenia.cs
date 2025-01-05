using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteka.Models
{
    public class Wypozyczenia
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Czytelnicy")]
        public int CzytelnikId { get; set; }

        public Czytelnicy Czytelnik { get; set; } // Nawigacja do Czytelnicy

        [Required]
        [ForeignKey("Ksiazki")]
        public int KsiazkaId { get; set; }

        public Ksiazki Ksiazka { get; set; } // Nawigacja do Ksiazki

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataWypozyczenia { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataZwrotu { get; set; } = DateTime.Now.AddDays(14); // Automatyczne wyliczenie daty zwrotu
    }
}
