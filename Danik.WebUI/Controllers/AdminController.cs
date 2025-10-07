using Danik.WebUI.Code;
using Danik.WebUI.Code.Domain;
using Danik.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Danik.WebUI.Controllers;

public class AdminController : AdmController
{


    public IActionResult TemplateBgImage()
    {
        return View(Registry.Current.TemplateBgImages.SelectAll());
    }
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

    public IActionResult Gallery(string? action=null, Guid[]? img=null, ImageFolder folder = ImageFolder.Загруженные_фото)
    {
        if (img != null && img.Length > 0)
        {
            foreach (var id in img) Registry.Current.Images.Delete(id);
            return RedirectToAction("Gallery", new { folder });
        }
        
        var images = Registry.Current.Images.SelectAll().Where(i => i.Folder == folder).ToList();
        images.Sort((i1, i2) => i2.Date.CompareTo(i1.Date));
        return View(new GalleryList(images.ToArray(), folder));
    }


    public IActionResult GalleryUpload(ImageFolder folder, IFormFile[] files)
    {
        foreach (IFormFile file in files)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var imgId = Image.Import(ms.ToArray(), file.FileName, folder);
        }
        return RedirectToAction("Gallery", new { folder });
    }
}

