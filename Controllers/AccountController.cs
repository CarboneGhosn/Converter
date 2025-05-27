using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace CurrencyRateViewer.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Exemplo simples: usuário e senha fixos
            if (username == "admin" && password == "Dani.0101!")
            {
                HttpContext.Session.SetString("username", username);
                return RedirectToAction("Index", "Currency");
            }

            ViewBag.Error = "Usuário ou senha inválidos";
            return View();
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            return RedirectToAction("Login");
        }
    }
}
