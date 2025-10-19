using System.Diagnostics;
using Danik.WebUI.Code;
using Danik.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Danik.WebUI.Controllers
{
    public class HomeController : AppController
    {
        public IActionResult Index(string? alias)
        {
            if (!string.IsNullOrWhiteSpace(alias))
            {
                Alias = alias;
                return RedirectToAction("Index");
            }
            var gi = Registry.Current.GalleryImages.SelectAll().Where(g => g.Alias == Alias).OrderBy(g => g.Sort).ToArray();
            return View(new StartPageModel(gi));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
