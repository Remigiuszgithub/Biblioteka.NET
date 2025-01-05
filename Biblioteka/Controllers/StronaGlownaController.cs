using Microsoft.AspNetCore.Mvc;

public class StronaGlownaController : Controller
{
    public IActionResult Index()
    {
        // Retrieve user details from session
        var username = HttpContext.Session.GetString("Username");
        var imie = HttpContext.Session.GetString("Imie");
        var nazwisko = HttpContext.Session.GetString("Nazwisko");
        var stanowisko = HttpContext.Session.GetString("Stanowisko");

        if (string.IsNullOrEmpty(username))
        {
            return RedirectToAction("Logowanie", "Konto");
        }

        ViewData["Powitanie"] = $"Witaj {imie} {nazwisko}";
        ViewData["Stanowisko"] = $"Twoje stanowisko: {stanowisko}";

        return View();
    }
}
