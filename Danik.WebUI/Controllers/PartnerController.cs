using Danik.WebUI.Code;
using Danik.WebUI.Code.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Danik.WebUI.Controllers;

public class PartnerController : PartController
{
    public IActionResult Index()
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
        return RedirectToAction("Index");
    }


    public IActionResult DeleteGalleryImage(Guid imageId)
    {
        Registry.Current.GalleryImages.Delete(imageId);
        return RedirectToAction("Index");
    }
}