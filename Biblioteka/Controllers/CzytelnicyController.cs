using Microsoft.AspNetCore.Mvc;
using Biblioteka.Dane;
using Biblioteka.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;
using iText.IO.Font.Constants;

namespace Biblioteka.Controllers
{
    public class CzytelnicyController : Controller
    {
        private readonly BibliotekaContext _context;

        public CzytelnicyController(BibliotekaContext context)
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

        public IActionResult Index()
        {
            var autoryzacjaError = SprawdzAutoryzacje();
            if (autoryzacjaError != null) return autoryzacjaError;

            var czytelnicy = _context.Czytelnicy.ToList();
            return View(czytelnicy);
        }

        public IActionResult Stworz()
        {
            var autoryzacjaError = SprawdzAutoryzacje();
            if (autoryzacjaError != null) return autoryzacjaError;

            return View();
        }

        [HttpPost]
        public IActionResult Stworz(Czytelnicy czytelnik)
        {
            var autoryzacjaError = SprawdzAutoryzacje();
            if (autoryzacjaError != null) return autoryzacjaError;

            if (ModelState.IsValid)
            {
                _context.Add(czytelnik);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(czytelnik);
        }

        public IActionResult Edytuj(int id)
        {
            var autoryzacjaError = SprawdzAutoryzacje();
            if (autoryzacjaError != null) return autoryzacjaError;

            var czytelnik = _context.Czytelnicy.Find(id);
            if (czytelnik == null)
            {
                return NotFound();
            }
            return View(czytelnik);
        }

        [HttpPost]
        public IActionResult Edytuj(Czytelnicy czytelnik)
        {
            var autoryzacjaError = SprawdzAutoryzacje();
            if (autoryzacjaError != null) return autoryzacjaError;

            if (ModelState.IsValid)
            {
                _context.Update(czytelnik);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(czytelnik);
        }

        public IActionResult Usun(int id)
        {
            var autoryzacjaError = SprawdzAutoryzacje();
            if (autoryzacjaError != null) return autoryzacjaError;

            var czytelnik = _context.Czytelnicy.Find(id);
            if (czytelnik == null)
            {
                return NotFound();
            }
            return View(czytelnik);
        }

        [HttpPost, ActionName("Usun")]
        public IActionResult UsunPotwierdz(int id)
        {
            var autoryzacjaError = SprawdzAutoryzacje();
            if (autoryzacjaError != null) return autoryzacjaError;

            var czytelnik = _context.Czytelnicy.Find(id);
            _context.Czytelnicy.Remove(czytelnik);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GenerateIdCard(int id)
        {
            var autoryzacjaError = SprawdzAutoryzacje();
            if (autoryzacjaError != null) return autoryzacjaError;

            var czytelnik = _context.Czytelnicy.Find(id);
            if (czytelnik == null)
            {
                return NotFound();
            }

            var pdfContent = CreateIdCardPdf(czytelnik);
            return File(pdfContent, "application/pdf", $"Identyfikator_{czytelnik.Id}.pdf");
        }

        private byte[] CreateIdCardPdf(Czytelnicy czytelnik)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new PdfWriter(stream))
                {
                    using (var pdf = new PdfDocument(writer))
                    {
                        var pageSize = new iText.Kernel.Geom.PageSize(370, 250); // Wymiary identyfikatora (szerokość, wysokość)
                        pdf.SetDefaultPageSize(pageSize);
                        var document = new Document(pdf);

                        // Dodanie tytułu "Karta biblioteczna"
                        var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD); // Tworzenie czcionki
                        document.Add(new Paragraph("Karta Biblioteczna")
                            .SetFont(font)
                            .SetFontSize(18) // Rozmiar czcionki tytułu
                            .SetTextAlignment(TextAlignment.CENTER)); // Wyśrodkowanie tytułu

                        // Definicja normalnej czcionki, używanej dla pozostałego tekstu
                        var normalFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA); // Tworzenie czcionki bez pogrubienia
                        document.Add(new Paragraph($"Numer identyfikacyjny czytelnika: {czytelnik.Id}")
                            .SetFont(normalFont)
                            .SetFontSize(12));
                        // Imię i Nazwisko w jednej linii
                        document.Add(new Paragraph($"Personalia: {czytelnik.Imie} {czytelnik.Nazwisko}")
                            .SetFont(normalFont)
                            .SetFontSize(12)); // Rozmiar czcionki dla imienia i nazwiska

                        document.Add(new Paragraph($"Adres zamieszkania: {czytelnik.Adres}")
                            .SetFont(normalFont)
                            .SetFontSize(12));
                        document.Add(new Paragraph($"Email: {czytelnik.Email}")
                            .SetFont(normalFont)
                            .SetFontSize(12));
                    }
                }
                return stream.ToArray();
            }
        }
    }
}
