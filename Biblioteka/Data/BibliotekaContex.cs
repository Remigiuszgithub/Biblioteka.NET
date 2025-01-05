using Microsoft.EntityFrameworkCore;
using Biblioteka.Models;

namespace Biblioteka.Dane
{
    public class BibliotekaContext : DbContext
    {
        public DbSet<Ksiazki> Ksiazki { get; set; }
        public DbSet<Czytelnicy> Czytelnicy { get; set; }
        public DbSet<Kategoria> Kategorie { get; set; }
        public DbSet<Wypozyczenia> Wypozyczenia { get; set; }
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }

        public BibliotekaContext(DbContextOptions<BibliotekaContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ksiazki>().HasKey(k => k.Id);
            modelBuilder.Entity<Czytelnicy>().HasKey(c => c.Id);
            modelBuilder.Entity<Kategoria>().HasKey(k => k.Id);
            modelBuilder.Entity<Uzytkownik>().HasKey(u => u.Id);
        }
    }
}
