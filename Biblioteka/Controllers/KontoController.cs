using Microsoft.AspNetCore.Mvc;
using Biblioteka.Dane;
using Biblioteka.Models;
using System.Linq;
using Microsoft.AspNetCore.Authentication;

public class KontoController : Controller
{
    private readonly BibliotekaContext _context;

    public KontoController(BibliotekaContext context)
    {
        _context = context;
    }

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

        return null;
    }

    [HttpGet]
    public IActionResult Logowanie()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Logowanie(string username, string haslo)
    {
        if (username == "admin" && haslo == "admin123")
        {
            HttpContext.Session.SetString("Username", "admin");
            HttpContext.Session.SetString("Imie", "Admin");
            HttpContext.Session.SetString("Nazwisko", "Biblioteka");
            HttpContext.Session.SetString("Stanowisko", "Administrator");

            return RedirectToAction("Index", "StronaGlowna");
        }

        var uzytkownik = _context.Uzytkownicy
            .FirstOrDefault(u => u.Username == username && u.PasswordHash == HashPassword(haslo));

        if (uzytkownik == null)
        {
            ViewBag.Blad = "Niepoprawna nazwa użytkownika lub hasło";
            return View();
        }

        HttpContext.Session.SetString("Username", uzytkownik.Username);
        HttpContext.Session.SetString("Imie", uzytkownik.Imie);
        HttpContext.Session.SetString("Nazwisko", uzytkownik.Nazwisko);
        HttpContext.Session.SetString("Stanowisko", uzytkownik.Stanowisko);

        return RedirectToAction("Index", "StronaGlowna");
    }

    public async Task<IActionResult> Wyloguj()
    {
        await HttpContext.SignOutAsync();
        HttpContext.Session.Clear(); // Wyczyść sesję
        return RedirectToAction("Logowanie", "Konto"); // Przekierowanie na stronę logowania
    }

    private static string HashPassword(string haslo)
    {
        using (var sha512 = System.Security.Cryptography.SHA512.Create())
        {
            var hashedBytes = sha512.ComputeHash(System.Text.Encoding.UTF8.GetBytes(haslo));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    public IActionResult Uzytkownicy()
    {
        var authResult = SprawdzAutoryzacje();
        if (authResult != null) return authResult;

        return View(_context.Uzytkownicy.ToList());
    }

    [HttpGet]
    public IActionResult DodajUzytkownika()
    {
        var authResult = SprawdzAutoryzacje();
        if (authResult != null) return authResult;

        return View();
    }

    [HttpPost]
    public IActionResult DodajUzytkownika(Uzytkownik nowyUzytkownik)
    {
        var authResult = SprawdzAutoryzacje();
        if (authResult != null) return authResult;

        if (ModelState.IsValid)
        {
            nowyUzytkownik.PasswordHash = HashPassword(nowyUzytkownik.PasswordHash);
            _context.Uzytkownicy.Add(nowyUzytkownik); // Nie ustawiamy Id
            _context.SaveChanges(); // Zapisz zmiany do bazy danych
            return RedirectToAction("Uzytkownicy");
        }

        return View(nowyUzytkownik);
    }
}
