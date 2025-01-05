using Microsoft.AspNetCore.Mvc;
using Biblioteka.Dane;
using Biblioteka.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Biblioteka.Controllers
{
    public class KategorieController : Controller
    {
        private readonly BibliotekaContext _context;

        public KategorieController(BibliotekaContext context)
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

        public async Task<IActionResult> Index()
        {
            var autoryzacjaError = SprawdzAutoryzacje();
            if (autoryzacjaError != null) return autoryzacjaError;

            var kategorie = await _context.Kategorie.ToListAsync();
            return View(kategorie);
        }

        public IActionResult Stworz()
        {
            var autoryzacjaError = SprawdzAutoryzacje();
            if (autoryzacjaError != null) return autoryzacjaError;

            return View();
        }

        [HttpPost]
        public IActionResult Stworz(Kategoria kategoria)
        {
            var autoryzacjaError = SprawdzAutoryzacje();
            if (autoryzacjaError != null) return autoryzacjaError;

            if (ModelState.IsValid)
            {
                _context.Add(kategoria);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(kategoria);
        }

        public IActionResult Edytuj(int id)
        {
            var autoryzacjaError = SprawdzAutoryzacje();
            if (autoryzacjaError != null) return autoryzacjaError;

            var kategoria = _context.Kategorie.Find(id);
            if (kategoria == null)
            {
                return NotFound();
            }
            return View(kategoria);
        }

        [HttpPost]
        public IActionResult Edytuj(Kategoria kategoria)
        {
            var autoryzacjaError = SprawdzAutoryzacje();
            if (autoryzacjaError != null) return autoryzacjaError;

            if (ModelState.IsValid)
            {
                _context.Update(kategoria);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(kategoria);
        }

        public IActionResult Usun(int id)
        {
            var autoryzacjaError = SprawdzAutoryzacje();
            if (autoryzacjaError != null) return autoryzacjaError;

            var kategoria = _context.Kategorie.FirstOrDefault(k => k.Id == id);
            if (kategoria == null)
            {
                return NotFound();
            }

            return View(kategoria);
        }

        [HttpPost]
        public IActionResult UsunConfirmed(int id)
        {
            var autoryzacjaError = SprawdzAutoryzacje();
            if (autoryzacjaError != null) return autoryzacjaError;

            var kategoria = _context.Kategorie.FirstOrDefault(k => k.Id == id);
            if (kategoria == null)
            {
                return NotFound();
            }

            _context.Kategorie.Remove(kategoria);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
