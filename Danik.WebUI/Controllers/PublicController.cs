using Microsoft.AspNetCore.Mvc;

namespace Danik.WebUI.Controllers;

public class PublicController : AppController
{
    public IActionResult Logout()
    {
        PutInSession("Admin", null);
        return Redirect("/");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login(string password)
    {
        if (password == "123")
        {
            PutInSession("Admin", "Admin");
            return Redirect("/Admin/");
        }
        return RedirectToAction("Login");
    }
}