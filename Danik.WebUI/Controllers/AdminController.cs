using Danik.WebUI.Code.Domain;
using Danik.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Danik.WebUI.Controllers;

public class AdminController : AdmController
{


    public IActionResult OrderView(Guid id)
    {
        return View(Registry.Current.Orders.Find(id));
    }
    public IActionResult Index(OrderStatus? status=null, string? q=null)
    {
        var selectAll = Registry.Current.Orders.SelectAll().ToList();
        if (status != null) selectAll = selectAll.Where(o => o.Status == status).ToList();
        if (!string.IsNullOrWhiteSpace(q))
        {
            selectAll = selectAll.Where(o => o.Number==q 
                                             || (o.Contact?.Name!=null && o.Contact.Name.Contains(q)) 
                                             || (o.Contact?.Phone!=null && o.Contact.Phone.Contains(q)) 
                                             || (o.Contact?.Email!=null && o.Contact.Email.Contains(q))
                                             ).ToList();
        }
        selectAll.Sort((a, b) => b.Number.CompareTo(a.Number));
        return View(new OrdersList(selectAll.ToArray(),status??OrderStatus.Создан, q));
    }

}

