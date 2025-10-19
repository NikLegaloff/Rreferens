using Danik.WebUI.Code.Helpers;
using Danik.WebUI.Code.ORM;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Danik.WebUI.Code.Domain;

public enum ImageFolder
{
    Загруженные_фото, Формы_камней, Клипарт_верт, Пользовательские_формы_камней, Фоны_шаблонов,
    Клипарт_гориз, Служебные, Портреты
}

public class Image : DomainObject
{
    public static ImageFolder[] Editable => [ImageFolder.Клипарт_верт, ImageFolder.Клипарт_гориз,ImageFolder.Служебные];
    public static string GetURL(Guid id) => $"/Images/GetImage/{id}";
    public static string GetTmb(Guid id) => $"/Images/GetTmb/{id}";
    public static Guid MaskImageId = new Guid("c312e8f6-78b1-4c67-b964-cffbc27b3f23");

    public required ImageFolder Folder { get; set; }
    public required DateTime Date { get; set; }
    public required string Name { get; set; }
    public int Size { get; set; }

    [JsonIgnore]
    public string URL => GetURL(Id);
    [JsonIgnore]
    public string TMB => GetTmb(Id);
    
    [JsonIgnore]
    //public string Path => $"{Env.Current.DataBasePath}Images\\{Id}." + Ext;
    public string Path => $"{Env.Current.DataBasePath}Images\\{Id.ToString().Substring(0,1)}\\{Id}." + Ext;
    [JsonIgnore]
    public  string TmbPath => $"{Env.Current.DataBasePath}Images\\{Id.ToString().Substring(0, 1)}\\{Id}_tmb.jpg";
    //public string TmbPath => $"{Env.Current.DataBasePath}Images\\{Id}_tmb.jpg";

    [JsonIgnore]
    public string Ext => Name.Contains(".") ? Name.Split('.').Last():"jpg";


    public void SaveImageData(byte[] data)
    {
        var dir = System.IO.Path.GetDirectoryName(Path);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir!);
        File.WriteAllBytes(Path, data);
    }

    public static Guid Import(byte[] data, string name, ImageFolder folder=ImageFolder.Загруженные_фото)
    {
        var img = new Image
        {
            Date = DateTime.Now,
            Name = name,
            Size = data.Length,
            Folder = folder
        };
        Registry.Current.Images.Save(img);
        img.SaveImageData(data);
        

        var clone = SixLabors.ImageSharp.Image.Load(data).Clone(ctx => ctx.Resize(new ResizeOptions
        {
            Size = new Size(200, 200),
            Mode = ResizeMode.Max
        }));
        clone.SaveAsJpeg(img.TmbPath);
        return img.Id;
    }


    public override void OnDelete()
    {
        File.Delete(TmbPath);
        File.Delete(Path);
        base.OnDelete();
    }
}