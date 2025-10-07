using Danik.WebUI.Code.Domain;
using Danik.WebUI.Models;
using Microsoft.AspNetCore.Mvc;


namespace Danik.WebUI.Controllers;

public class WizController : AppController
{
    [HttpGet]
    public IActionResult Step1(Guid? id)
    {
        var order = id != null ? Registry.Current.Orders.Find(id.Value) : new Order(){Number = "",Persons = 1,Type = StoneType.Вертикальный,Options = new OrderOptions{Size = 40}};
        return View(order);
    }
    [HttpPost]
    public IActionResult Step1(Guid id, StoneType type,int count, int ddlSize, IFormFile? ownFormFile,IFormFile[]? personFile, Guid? stoneFormId)
    {
        Order? order;
        if(id==Guid.Empty)
            order = new Order
            {
                Number = (Registry.Current.SystemSettings.GetOrderNumber()), Persons = count,
                Type = type, 
                Date = DateTime.Now, 
                Status = OrderStatus.Создаётся,
                Options =
                {
                    Size = ddlSize
                },
            };
        else
            order = Registry.Current.Orders.Find(id);
        if (order == null) throw new BusinessException("Order not found ");
        order.Type = type;
        order.Persons = count;
        order.Options.Size = ddlSize;
        order.StoneForm=stoneFormId;

        if (ownFormFile != null && ownFormFile.Length > 0)
        {
            using var ms = new MemoryStream();
            ownFormFile.CopyTo(ms);
            var imgId = Image.Import(ms.ToArray(), ownFormFile.FileName, ImageFolder.Пользовательские_формы_камней);
            if(order.StoreFormImage!=null) Registry.Current.Images.Delete(order.StoreFormImage.Value);
            order.StoreFormImage = imgId;
        }


        if (personFile!=null && personFile.Length> 0)
        {
            var images = new List<Guid>();

            foreach (var file in personFile)
            {
                using var ms = new MemoryStream();
                file.CopyTo(ms);
                images.Add(Image.Import(ms.ToArray(), file.FileName));
            }
            if(order.PortraitImages!=null) foreach (var imId in order.PortraitImages) Registry.Current.Images.Delete(imId);
                
            order.PortraitImages = images.ToArray();
        }

        Registry.Current.Orders.Save(order);

        return RedirectToAction("Step3",new {order.Id });
       // return RedirectToAction("Step2",new {orderId = order.Id, imageId = order.PortraitImages[0] });
    }

    // -------------- STEP 2 ----------------
    [HttpGet]
    public IActionResult Step2(Guid id, Guid imageId)
    {
        var order = Registry.Current.Orders.Find(id);
        var image = Registry.Current.Images.Find(imageId);
        if (image == null) throw new Exception("Image not found ");
        if (order== null) throw new Exception("Order not found ");

        return View(new WizStep2(order,imageId, image.URL));
    }

    
    [HttpPost]
    public IActionResult Step2(Guid id, Guid imageId, int x, int y, int w, int h, int r)
    {
        var order = Registry.Current.Orders.Find(id);
        var image = Registry.Current.Images.Find(imageId);
        if (image == null) throw new Exception("Image not found ");
        if (order == null) throw new Exception("Order not found ");

        return RedirectToAction("Step3", new { order.Id});
    }

    // -------------- STEP 3 ----------------
    [HttpGet]
    public IActionResult Step3(Guid id)
    {
        var order = Registry.Current.Orders.Find(id);
        if (order == null) throw new Exception("Order not found ");
        if (order.PortraitImages == null) return RedirectToAction("Step1", new { id });
        return View(order);
    }

    [HttpPost]
    public IActionResult Step3(Guid id, PersonInfo[] info)
    {
        var order = Registry.Current.Orders.Find(id);
        if (order == null) throw new Exception("Order not found ");
        order.PersonInfos = info;
        Registry.Current.Orders.Save(order);
        return RedirectToAction("Step5",new {id});
    }

    // -------------- STEP 4 ----------------
    public IActionResult Step4(Guid id)
    {
        var order = Registry.Current.Orders.Find(id);
        if (order == null) throw new Exception("Order not found ");
        return View(Registry.Current.Templates.SelectAll().First());
    }
    // -------------- STEP 5 ----------------
    [HttpGet]
    public IActionResult Step5(Guid id)
    {
        var order = Registry.Current.Orders.Find(id);
        if (order == null) throw new Exception("Order not found ");
        return View(order);
    }
    [HttpPost]
    public IActionResult Step5(Guid id, Order data)
    {
        var order = Registry.Current.Orders.Find(id);
        if (order == null) throw new Exception("Order not found ");

        order.Contact = data.Contact;
        order.Comment = data.Comment;
        order.Options = data.Options;

        var images = new List<Guid>();
        foreach (var file in Request.Form.Files)
        {
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            images.Add(Image.Import(ms.ToArray(), file.FileName));
        }

        if(images.Count>0) order.ExampleImages = images.ToArray();

        Registry.Current.Orders.Save(order);
        return RedirectToAction("StepConfirm", new { id });
    }

    // -------------- STEP CONFIRM ----------------
    public IActionResult StepConfirm(Guid id)
    {
        var order = Registry.Current.Orders.Find(id);
        if (order == null) throw new Exception("Order not found ");
        return View(order);
    }

    public IActionResult DoConfirm(Guid id)
    {
        var order = Registry.Current.Orders.Find(id);
        if (order == null) throw new Exception("Order not found ");
        order.Status=OrderStatus.Создан;
        Registry.Current.Orders.Save(order);

        return RedirectToAction("StepConfirm", new {id});
    }

}