using Danik.WebUI.Code.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Danik.WebUI.Controllers;

public class ImagesController : Controller
{
    public IActionResult GetImage(Guid id)
    {
        var img = Registry.Current.Images.Find(id);
        if(img == null) return NotFound();
        var path = img.Path;
        if(!System.IO.File.Exists(path)) return NotFound();
        return File(System.IO.File.ReadAllBytes(path), "image/" + img.Ext.Replace("jpg","jpeg"));
    }
    public IActionResult GetTmb(Guid id)
    {
        var img = Registry.Current.Images.Find(id);
        if (img == null) return NotFound();
        return File(System.IO.File.ReadAllBytes(img.TmbPath), "image/jpeg");
    }
}