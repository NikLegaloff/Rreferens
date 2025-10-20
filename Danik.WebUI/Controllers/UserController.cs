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
        var u = CurrentUser;
        if (u.Password != currentPassword.MD5()) return View (new ChangePasswordModel("Неправильный текущий пароль", "danger"));
        if (newPassword != confirmNewPassword) return View(new ChangePasswordModel("Пароли не совпадают", "danger"));
        u.Password = newPassword.MD5();
        Registry.Current.Users.Save(u);
        return View(new ChangePasswordModel("Пароль успешно изменён", "success"));
    }

    [HttpGet]
    public IActionResult ChangePassword(string? msg)
    {
        return View(new ChangePasswordModel(msg, "info"));
    }
}