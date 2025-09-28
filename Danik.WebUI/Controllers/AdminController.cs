using Danik.WebUI.Code.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Danik.WebUI.Controllers;

public class AdminController : AdmController
{


    public IActionResult OrderView(Guid id)
    {
        return View(Registry.Current.Orders.Find(id));
    }
    public IActionResult Index()
    {
        var selectAll = Registry.Current.Orders.SelectAll().ToList();
        selectAll.Sort((a, b) => b.Number.CompareTo(a.Number));
        return View(selectAll.ToArray());
    }

}

