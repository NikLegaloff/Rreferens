using Danik.WebUI.Code;
using Danik.WebUI.Code.ORM;
using Danik.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Danik.WebUI.Controllers;

public class UserController : PartController
{
    [HttpPost]
    public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmNewPassword)
    {
        var alias = GetAlias();
        var u = CurrentUser;
        if (u.Password != currentPassword.MD5()) return View (new ChangePasswordModel("Неправильный текущий пароль", "danger",alias));
        if (newPassword != confirmNewPassword) return View(new ChangePasswordModel("Пароли не совпадают", "danger", alias));
        u.Password = newPassword.MD5();
        Registry.Current.Users.Save(u);
        return View(new ChangePasswordModel("Пароль успешно изменён", "success", alias));
    }

    private string GetAlias()
    {
        var alias = Registry.Current.Partners.Find(CurrentUser.PartnerId)!.Alias;
        return alias;
    }

    [HttpGet]
    public IActionResult ChangePassword(string? msg)
    {
        return View(new ChangePasswordModel(msg, "info", GetAlias()));
    }
}