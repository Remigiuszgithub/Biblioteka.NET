using Microsoft.AspNetCore.Mvc;
using Biblioteka.Models;
using Biblioteka.Dane;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Biblioteka.Controllers
{
    public class WypozyczeniaController : Controller
    {
        private readonly BibliotekaContext _context;

        public WypozyczeniaController(BibliotekaContext context)
        {
            _context = context;
        }

        // Nowa metoda pomocnicza do zarządzania sesją
        private IActionResult SprawdzAutoryzacje()
        {
            var username = HttpContext.Session.GetString("Username");
            var imie = HttpContext.Session.GetString("Imie");
            var nazwisko = HttpContext.Session.GetString("Nazwisko");
            var stanowisko = HttpContext.Session.GetString("Stanowisko");

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Logowanie", "Konto");
            }

            return null; // W przypadku zalogowanego użytkownika zwracana jest wartość null, co oznacza brak przekierowania
        }

        // Akcja wyświetlająca formularz wypożyczenia
        public IActionResult Wypozycz(int ksiazkaId)
        {
            var autoryzacjaError = SprawdzAutoryzacje();
            if (autoryzacjaError != null) return autoryzacjaError;

            var ksiazka = _context.Ksiazki.Find(ksiazkaId);
            if (ksiazka == null)
            {
                return NotFound("Książka nie istnieje.");
            }

            ViewBag.KsiazkaTytul = ksiazka.Tytul;
            ViewBag.Czytelnicy = _context.Czytelnicy
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Imie} {c.Nazwisko}"
                }).ToList();

            var model = new Wypozyczenia
            {
                KsiazkaId = ksiazkaId,
                DataWypozyczenia = DateTime.Now,
                DataZwrotu = DateTime.Now.AddDays(14)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult WypozyczPotwierdz(Wypozyczenia wypozyczenie)
        {
            var autoryzacjaError = SprawdzAutoryzacje();
            if (autoryzacjaError != null) return autoryzacjaError;

            if (ModelState.IsValid)
            {
                wypozyczenie.DataWypozyczenia = DateTime.Now;
                wypozyczenie.DataZwrotu = DateTime.Now.AddDays(14);

                _context.Wypozyczenia.Add(wypozyczenie);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            _context.Wypozyczenia.Add(wypozyczenie);
            _context.SaveChanges();

            ViewBag.Czytelnicy = _context.Czytelnicy
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Imie} {c.Nazwisko}"
                }).ToList();

            ViewBag.KsiazkaTytul = _context.Ksiazki
                .Where(k => k.Id == wypozyczenie.KsiazkaId)
                .Select(k => k.Tytul)
                .FirstOrDefault();

            return RedirectToAction("Index");
        }

        // Akcja wyświetlająca listę wypożyczonych książek
        public IActionResult Index()
        {
            var autoryzacjaError = SprawdzAutoryzacje();
            if (autoryzacjaError != null) return autoryzacjaError;

            var wypozyczenia = _context.Wypozyczenia
                .Include(w => w.Czytelnik)
                .Include(w => w.Ksiazka)
                .Select(w => new Wypozyczenia
                {
                    Id = w.Id,
                    DataWypozyczenia = w.DataWypozyczenia,
                    DataZwrotu = w.DataZwrotu,
                    Czytelnik = w.Czytelnik,
                    Ksiazka = w.Ksiazka
                })
                .ToList();

            return View(wypozyczenia);
        }
    }
}
