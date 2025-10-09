using Danik.WebUI.Code;
using Danik.WebUI.Code.Domain;
using Danik.WebUI.Code.ORM;
using Danik.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Image = Danik.WebUI.Code.Domain.Image;


namespace Danik.WebUI.Controllers;

public class WizController : AppController
{
    [HttpGet]
    public IActionResult Step1(Guid? id)
    {
        Order? order;
        if (id != null)
            order = Registry.Current.Orders.Find(id.Value);
        else
        {
            var stoneForms = Registry.Current.StoneForms.SelectAll().ToList();
            stoneForms.Sort();
            order = new Order()
            {
                Number = "", Persons = 1, Type = StoneType.Вертикальный,
                Options = new OrderOptions { Size = 40 }
            };
        }
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
                Options = new OrderOptions(),
            };
        else
            order = Registry.Current.Orders.Find(id);
        if (order == null) throw new BusinessException("Order not found ");
        var changeTemplate = order.Type != type || order.Persons != count;

        order.Type = type;
        order.Persons = count;
        
        order.Options.Size = ddlSize;
        order.StoneForm=stoneFormId;
        
        if (order.TemplateId==null || changeTemplate)
        {
            var template = Registry.Current.Templates.SelectAll().FirstOrDefault(t => t.Type == type && t.Persons == count);
            if (template == null) throw new BusinessException("No template for this type and persons count");
            order.TemplateId = template.Id;
            order.TemplateData.Template=template.Data;
        }
        
        if (order.TemplateData.PersonInfos == null || order.TemplateData.PersonInfos.Length != count)
        {
            var infos = new PersonInfo[count];
            for (int i = 0; i < count; i++)
            {
                infos[i] = order.TemplateData.PersonInfos!=null && i<order.TemplateData.PersonInfos.Length ? order.TemplateData.PersonInfos[i] : 
                    new PersonInfo
                    {
                        F = "",
                        I = "",
                        O = "",
                        Birth = "",
                        Dead = "",
                    };
            }
            order.TemplateData.PersonInfos = infos;
        }

        if (ownFormFile is { Length: > 0 })
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
            for (int i = 0; i < order.PortraitImages.Length && i < count; i++)
            {
                order.TemplateData.PersonInfos[i].ImageId = order.PortraitImages[i];
            }

        }

        Registry.Current.Orders.Save(order);

        //return RedirectToAction("Step4",new {order.Id });
         return RedirectToAction("Step2",new {orderId = order.Id, imageId = order.PortraitImages[0] });
    }

    // -------------- STEP 2 ----------------
    [HttpGet]
    public IActionResult Step2(Guid id, Guid imageId)
    {
        var order = Registry.Current.Orders.Find(id);
        var image = Registry.Current.Images.Find(imageId);
        if (image == null) throw new Exception("Image not found ");
        if (order== null) throw new Exception("Order not found ");

        return View(new WizStep2(order,image));
    }

    
    [HttpPost]
    public IActionResult Step2(Guid id, Guid imageId, int x, int y, int w, int h)
    {
        var order = Registry.Current.Orders.Find(id);
        var image = Registry.Current.Images.Find(imageId);
        if (image == null) throw new Exception("Image not found ");
        if (order == null) throw new Exception("Order not found ");
        if (order.PortraitImages == null) throw new Exception("Order has no images");
        var list = order.PortraitImages.ToList();
        var index = list.IndexOf(id);

        using var img = SixLabors.ImageSharp.Image.Load<Rgba32>(image.Path);
        img.Mutate(ctx => ctx.Crop(new Rectangle(x, y, w, h)));
        // resize to 600x800 
        img.Mutate(ctx => ctx.Resize(new ResizeOptions
        {
            Size = new Size(600, 800),
            Mode = ResizeMode.Max
        }));
        var alphai = Registry.Current.Images.Find(Image.MaskImageId);
        if (alphai != null)
        {
            using var mask = SixLabors.ImageSharp.Image.Load<L8>(alphai.Path);
            img.Mutate(ctx =>
            {
                for (int y = 0; y < img.Height; y++)
                {
                    for (int x = 0; x < img.Width; x++)
                    {
                        var pixel = img[x, y];
                        var maskPixel = mask[x, y];
                        pixel.A = maskPixel.PackedValue;
                        img[x, y] = pixel;
                    }
                }
            });
        }
        
        using var ms = new MemoryStream();
        img.SaveAsJpeg(ms);
        var pid = Image.Import(ms.ToArray(),"Заказ №" + order + "-" + (index+1) + ".jpg",ImageFolder.Портреты);
        order.TemplateData.PersonInfos[index].ImageId = pid;

        if (index+1==list.Count) return RedirectToAction("Step4", new { order.Id});
        return RedirectToAction("Step2", new { orderId = order.Id, imageId = list[index + 1] });
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
    public IActionResult Step3(Guid id, PersonInfo[] info, string? epitaph=null)
    {
        var order = Registry.Current.Orders.Find(id);
        if (order == null) throw new Exception("Order not found ");
        order.TemplateData.PersonInfos = info;
        order.TemplateData.Epitaph = epitaph;
        Registry.Current.Orders.Save(order);
        return RedirectToAction("Step4",new {id});
    }

    // -------------- STEP 4 ----------------
    public IActionResult Step4(Guid id)
    {
        var order = Registry.Current.Orders.Find(id);
        if (order == null) throw new Exception("Order not found ");
        if (order.TemplateData.Template == null)
        {
            var template = Registry.Current.Templates.SelectFor(order).FirstOrDefault();
            order.TemplateData.Template = template?.Data;
            order.TemplateId= template?.Id;
            Registry.Current.Orders.Save(order);
        }
        return View(order);
    }

    [HttpPost]
    public IActionResult Step4Save(Guid id, string json)
    {
        var data = json.ToSubj<OrderTemplateData>();
        var order = Registry.Current.Orders.Find(id);
        if (order == null) throw new Exception("Order not found ");
        if (data == null) throw new Exception("Data is null");
        order.TemplateData = data;
        Registry.Current.Orders.Save(order);
        return RedirectToAction("Step5", new { id });
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