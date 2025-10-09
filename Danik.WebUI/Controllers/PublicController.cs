using Danik.WebUI.Code;
using Danik.WebUI.Code.ORM;
using Microsoft.AspNetCore.Mvc;

namespace Danik.WebUI.Controllers;

public class PublicController : AppController
{
    public IActionResult Logout()
    {
        PutInSession("User", null);
        return Redirect("/");
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login(string? email, string? password, string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password)) return RedirectToAction("Login");
        var pwd = password.MD5();
        var user = Registry.Current.Users.SelectAll().FirstOrDefault(u => u.Email.ToLower() == email.ToLower() && u.Password == pwd);
        if (user!=null)
        {
            PutInSession("User", user);
            if (user.IsAdmin)
            {
                if (!string.IsNullOrWhiteSpace(returnUrl) && returnUrl.StartsWith("/Admin/")) return Redirect(returnUrl);
                return Redirect("/Admin/");
            }
            if (!string.IsNullOrWhiteSpace(returnUrl) && returnUrl.StartsWith("/Partner/")) return Redirect(returnUrl);
            return Redirect("/Partner/");
        }
        return RedirectToAction("Login");
    }
}