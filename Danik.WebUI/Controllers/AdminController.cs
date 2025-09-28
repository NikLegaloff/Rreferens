using Danik.WebUI.Code.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Danik.WebUI.Controllers;

public class AdminController : Controller
{
    public IActionResult Index()
    {
        var selectAll = Registry.Current.Orders.SelectAll().ToList();
        selectAll.Sort((a, b) => b.Number.CompareTo(a.Number));
        return View(selectAll.ToArray());
    }

}