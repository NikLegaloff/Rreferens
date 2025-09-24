using Danik.WebUI.Code.Domain;
using Danik.WebUI.Models;
using Microsoft.AspNetCore.Mvc;


namespace Danik.WebUI.Controllers;

public class WizController : Controller
{
    [HttpGet]
    public IActionResult Step1()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Step1(bool isVert,int count, int size)
    {
        var order = new Order
        {
            Number = "O-" + (Registry.Current.Orders.SelectAll().Length + 10001), Persons = count,
            Options =
            {
                IsVert = isVert,
                Size = size
            }
        };
        var images = new List<Guid>();
        foreach (var file in Request.Form.Files)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            images.Add(Image.Import(ms.ToArray(), file.FileName));
        }
        order.PortraitImages = images.ToArray();
        Registry.Current.Orders.Save(order);

        return RedirectToAction("Step3",new {orderId = order.Id });
       // return RedirectToAction("Step2",new {orderId = order.Id, imageId = order.PortraitImages[0] });
    }

    // -------------- STEP 2 ----------------
    [HttpGet]
    public IActionResult Step2(Guid orderId, Guid imageId)
    {
        var order = Registry.Current.Orders.Find(orderId);
        var image = Registry.Current.Images.Find(imageId);
        if (image == null) throw new Exception("Image not found ");
        if (order== null) throw new Exception("Order not found ");

        return View(new WizStep2(order,imageId, image.URL));
    }

    
    [HttpPost]
    public IActionResult Step2(Guid orderId, Guid imageId, int x, int y, int w, int h, int r)
    {
        var order = Registry.Current.Orders.Find(orderId);
        var image = Registry.Current.Images.Find(imageId);
        if (image == null) throw new Exception("Image not found ");
        if (order == null) throw new Exception("Order not found ");

        return RedirectToAction("Step3", new { orderId = order.Id});
    }

    // -------------- STEP 3 ----------------
    [HttpGet]
    public IActionResult Step3(Guid orderId)
    {
        var order = Registry.Current.Orders.Find(orderId);
        if (order == null) throw new Exception("Order not found ");
        if (order.PortraitImages == null) return RedirectToAction("Step4", new { orderId });
        return View(order);
    }

    [HttpPost]
    public IActionResult Step3(Guid orderId, PersonInfo[] info)
    {
        var order = Registry.Current.Orders.Find(orderId);
        if (order == null) throw new Exception("Order not found ");
        order.PersonInfos = info;
        Registry.Current.Orders.Save(order);
        return RedirectToAction("Step4",new {orderId});
    }

    // -------------- STEP 4 ----------------
    public IActionResult Step4(Guid orderId)
    {
        var order = Registry.Current.Orders.Find(orderId);
        if (order == null) throw new Exception("Order not found ");
        return View();
    }
}