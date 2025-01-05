using System.ComponentModel.DataAnnotations;

namespace Biblioteka.Models
{
    public class Czytelnicy
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Imię nie może być dłuższe niż 50 znaków.")]
        public string Imie { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Nazwisko nie może być dłuższe niż 50 znaków.")]
        public string Nazwisko { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Adres nie może być dłuższy niż 100 znaków.")]
        public string Adres { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Proszę podać poprawny adres email.")]
        public string Email { get; set; }
    }
}
