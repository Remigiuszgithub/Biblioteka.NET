using System.ComponentModel.DataAnnotations;

namespace Biblioteka.Models
{
    public class Kategoria
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nazwa { get; set; }
    }
}
