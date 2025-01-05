using System.ComponentModel.DataAnnotations;

namespace Biblioteka.Models
{
    public class Ksiazki
    {
        public int Id { get; set; }
        public string Tytul { get; set; }
        public string Autor { get; set; }
        public string Kategoria { get; set; }
        public bool Dostepna { get; set; } = true;
    }
}
