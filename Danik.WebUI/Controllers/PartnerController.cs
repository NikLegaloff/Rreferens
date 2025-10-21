using Danik.WebUI.Code;
using Danik.WebUI.Code.Domain;
using Danik.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Danik.WebUI.Controllers;


public class PartnerController : PartController
{
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Orders(OrderStatus? status = null, string? q = null)
    {
        var partnerId = CurrentUser.PartnerId;
        var selectAll = Registry.Current.Orders.SelectAll().Where(o => o.PartnerId == partnerId).ToList();
        if (status != null) selectAll = selectAll.Where(o => o.Status == status).ToList();
        
        if (!string.IsNullOrWhiteSpace(q))
        {
            selectAll = selectAll.Where(o => o.Number == q
                                             || (o.Contact?.Name != null && o.Contact.Name.Contains(q))
                                             || (o.Contact?.Phone != null && o.Contact.Phone.Contains(q))
                                             || (o.Contact?.Email != null && o.Contact.Email.Contains(q))
            ).ToList();
        }
        selectAll.Sort((a, b) => b.Number.CompareTo(a.Number));
        return View(new OrdersList(selectAll.ToArray(), status, q, partnerId));

    }
    public IActionResult OrderView(Guid id)
    {
        return View(Registry.Current.Orders.Find(id));
    }

    public IActionResult OrderDelete(Guid id)
    {
        Registry.Current.Orders.Delete(id);
        return RedirectToAction("Orders");
    }

    public IActionResult Gallery()
    {
        return View(Registry.Current.GalleryImages.SelectByPartnerId(CurrentUser.PartnerId));
    }

    
    public IActionResult UploadGalleryImages(IFormFile[] files)
    {
        foreach (IFormFile file in files)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var imgId = GalleryImage.Import(ms.ToArray(), CurrentUser.PartnerId);
        }
        return RedirectToAction("Gallery");
    }


    public IActionResult DeleteGalleryImage(Guid imageId)
    {
        Registry.Current.GalleryImages.Delete(imageId);
        return RedirectToAction("Gallery");
    }
}