using Biblioteka.Dane;
using Biblioteka.Models;
using Microsoft.AspNetCore.Mvc;

public class KsiazkiController : Controller
{
    private readonly BibliotekaContext _context;

    public KsiazkiController(BibliotekaContext context)
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

        var ksiazki = _context.Ksiazki.ToList();
        ViewBag.Kategorie = _context.Kategorie.Select(k => k.Nazwa).ToList(); // Uaktualnia ViewBag.Kategorie
        return View(ksiazki);
    }

    public IActionResult Stworz()
    {
        var autoryzacjaError = SprawdzAutoryzacje();
        if (autoryzacjaError != null) return autoryzacjaError;

        ViewBag.Kategorie = _context.Kategorie.Select(k => k.Nazwa).ToList(); // Uaktualnia ViewBag.Kategorie
        return View();
    }

    [HttpPost]
    public IActionResult Stworz(Ksiazki ksiazka)
    {
        var autoryzacjaError = SprawdzAutoryzacje();
        if (autoryzacjaError != null) return autoryzacjaError;

        if (ModelState.IsValid)
        {
            _context.Add(ksiazka);
            _context.SaveChanges();

            ViewBag.Kategorie = _context.Kategorie.Select(k => k.Nazwa).ToList(); // Uaktualnia ViewBag.Kategorie po dodaniu nowej książki
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Kategorie = _context.Kategorie.Select(k => k.Nazwa).ToList(); // Uaktualnia ViewBag.Kategorie w przypadku błędu walidacji
        return View(ksiazka);
    }

    public IActionResult Edytuj(int id)
    {
        var autoryzacjaError = SprawdzAutoryzacje();
        if (autoryzacjaError != null) return autoryzacjaError;

        var ksiazka = _context.Ksiazki.Find(id);
        if (ksiazka == null)
        {
            return NotFound();
        }

        ViewBag.Kategorie = _context.Kategorie.Select(k => k.Nazwa).ToList(); // Uaktualnia ViewBag.Kategorie przed renderowaniem formularza edycji
        return View(ksiazka);
    }

    [HttpPost]
    public IActionResult Edytuj(Ksiazki ksiazka)
    {
        var autoryzacjaError = SprawdzAutoryzacje();
        if (autoryzacjaError != null) return autoryzacjaError;

        if (ModelState.IsValid)
        {
            _context.Update(ksiazka);
            _context.SaveChanges();

            ViewBag.Kategorie = _context.Kategorie.Select(k => k.Nazwa).ToList(); // Uaktualnia ViewBag.Kategorie po edycji książki
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Kategorie = _context.Kategorie.Select(k => k.Nazwa).ToList(); // Uaktualnia ViewBag.Kategorie w przypadku błędu walidacji
        return View(ksiazka);
    }

    public IActionResult Usun(int id)
    {
        var autoryzacjaError = SprawdzAutoryzacje();
        if (autoryzacjaError != null) return autoryzacjaError;

        var ksiazka = _context.Ksiazki.Find(id);
        if (ksiazka == null)
        {
            return NotFound();
        }
        return View(ksiazka);
    }

    [HttpPost, ActionName("Usun")]
    public IActionResult UsunPotwierdz(int id)
    {
        var autoryzacjaError = SprawdzAutoryzacje();
        if (autoryzacjaError != null) return autoryzacjaError;

        var ksiazka = _context.Ksiazki.Find(id);
        if (ksiazka == null)
        {
            return NotFound();
        }

        _context.Ksiazki.Remove(ksiazka);
        _context.SaveChanges();

        ViewBag.Kategorie = _context.Kategorie.Select(k => k.Nazwa).ToList(); // Uaktualnia ViewBag.Kategorie po usunięciu książki
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Wypozycz(int id)
    {
        var autoryzacjaError = SprawdzAutoryzacje();
        if (autoryzacjaError != null) return autoryzacjaError;

        var ksiazka = _context.Ksiazki.Find(id);
        if (ksiazka == null)
        {
            return NotFound();
        }

        // Zaktualizuj dostępność książki
        ksiazka.Dostepna = false;
        _context.SaveChanges();

        // Przekierowanie do formularza wypożyczenia
        return RedirectToAction("Wypozycz", "Wypozyczenia", new { ksiazkaId = id });
    }

    public IActionResult Zwroc(int id)
    {
        var autoryzacjaError = SprawdzAutoryzacje();
        if (autoryzacjaError != null) return autoryzacjaError;

        // Znajdź książkę
        var ksiazka = _context.Ksiazki.FirstOrDefault(k => k.Id == id);
        if (ksiazka == null)
        {
            return NotFound();
        }

        // Ustaw książkę jako dostępną
        ksiazka.Dostepna = true;

        // Usuń wypożyczenie powiązane z tą książką
        var wypozyczenie = _context.Wypozyczenia.FirstOrDefault(w => w.KsiazkaId == id);
        if (wypozyczenie != null)
        {
            _context.Wypozyczenia.Remove(wypozyczenie);
        }

        // Zapisz zmiany w bazie
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}
