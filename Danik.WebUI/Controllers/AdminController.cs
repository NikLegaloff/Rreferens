using Danik.WebUI.Code;
using Danik.WebUI.Code.Domain;
using Danik.WebUI.Code.ORM;
using Danik.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Danik.WebUI.Controllers;

public class AdminController : AdmController
{


    public IActionResult TemplateEdit(Guid id)
    {
        return View(Registry.Current.Templates.Find(id));
    }

    public IActionResult TemplateCreate(StoneType type,int persons)
    {
        if (type == StoneType.Сплит && persons == 1) throw new BusinessException("Не катит :)");
        var template = Registry.Current.Templates.SelectAll().FirstOrDefault(t=>t.Type==type && t.Persons==persons);
        if (template == null)
        {
            var pp = new List<TemplatePortrait>();
            var tt = new List<TemplateText>();
            for (int i =1; i <= persons; i++)
            {
                pp.Add(new TemplatePortrait
                {
                    Num = i * 5 + 1,
                    Area = new Area(100 + i * 150, 100, 350),
                    ImageId = "22aa5d22-9a48-4417-90c6-6c75de566cb1"
                });
                tt.Add(new TemplateText
                {
                    Num = i*5 + 2,
                    Area = new Area(100 + i * 150, 270, 200),
                    Text = $"{{фамилия{i}}}",
                    Size = 34,
                    Bold = true
                });
                tt.Add(new TemplateText
                {
                    Num = i*5 + 3,
                    Area = new Area(100 + i * 150, 270, 200),
                    Text = $"{{имя{i}}} {{отчество{i}}}",
                    Size = 28,
                    Bold = true
                });
                tt.Add(new TemplateText
                {
                    Num = i*5 + 4,
                    Area = new Area(100 + i * 150, 300, 200),
                    Text = $"{{датар{i}}} - {{датас{i}}}",
                    Size = 22,
                    Bold = false
                });
            }
            tt.Add(new TemplateText
            {
                Num = 51,
                Area = new Area(100 + 2 * 150, 300, 120),
                Text = $"{{эпитафия}}",
                Size = 22,
                Bold = false
            });

            template = new Template { 
                Type = type, 
                Persons = persons, 
                Data = new TemplateData
                {
                    Portraits = pp.ToArray(), 
                    Texts = tt.ToArray(),
                    BgImageId = Registry.Current.TemplateBgImages.Get(type, persons).ImageId,
                    Images =
                    [
                        new TemplateImage
                        {
                            Area = new Area(100 + 2 * 150, 100, 120),
                            IsVert = true,
                            ImageId = "f677a457-0752-4a96-a963-2e4df2e409cc",
                            Num = 31
                        }
                    ]
                } };
            Registry.Current.Templates.Save(template);
        }
        return RedirectToAction("TemplateEdit", new { template.Id });
    }

    public IActionResult Templates()
    {
        return View(Registry.Current.Templates.SelectAll());
    }

    public IActionResult TemplateBgImage()
    {
        return View(Registry.Current.TemplateBgImages.SelectAll());
    }

    public IActionResult OrderDelete(Guid id)
    {
        Registry.Current.Orders.Delete(id);
        return RedirectToAction("Index");
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

    public IActionResult TemplateSave(Guid id, string data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        var template = Registry.Current.Templates.Find(id);
        if (template == null) return NotFound();
        template.Data = data.ToSubj<TemplateData>();
        Registry.Current.Templates.Save(template);
        return RedirectToAction("Templates");
    }
}

