using Danik.WebUI.Code.ORM;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Danik.WebUI.Code.Domain;

public class Image : DomainObject
{
    public static string GetURL(Guid id) => $"/Images/GetImage/{id}";
    public static string GetTmb(Guid id) => $"/Images/GetTmb/{id}";

    public required DateTime Date { get; set; }
    public required string Name { get; set; }
    public int Size { get; set; }

    [JsonIgnore]
    public string URL => GetURL(Id);
    [JsonIgnore]
    public string TMB => GetTmb(Id);
    
    [JsonIgnore]
    public string Path => $"{Env.Current.DataBasePath}Images\\{Id}." + Ext;
    //public string Path => $"{Env.Current.DataBasePath}Images\\{Id.ToString().Substring(0,1)}\\{Id}." + Ext;
    [JsonIgnore]
    //public string TmbPath => $"{Env.Current.DataBasePath}Images\\{Id.ToString().Substring(0, 1)}\\{Id}_tmb.jpg";
    public string TmbPath => $"{Env.Current.DataBasePath}Images\\{Id}_tmb.jpg";

    [JsonIgnore]
    public string Ext => Name.Contains(".") ? Name.Split('.').Last():"jpg";


    public void SaveImageData(byte[] data)
    {
        var dir = System.IO.Path.GetDirectoryName(Path);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir!);
        File.WriteAllBytes(Path, data);
    }

    public static Guid Import(byte[] data, string name)
    {
        var img = new Image() { Date = DateTime.Now, Name = name, Size=data.Length };
        Registry.Current.Images.Save(img);
        img.SaveImageData(data);

        SixLabors.ImageSharp.Image.Load(data).Clone(ctx => ctx.Resize(new ResizeOptions
        {
            Size = new Size(200, 200),
            Mode = ResizeMode.Max
        })).SaveAsJpeg(img.TmbPath);
        return img.Id;
    }


    public override void OnDelete()
    {
        File.Delete(TmbPath);
        File.Delete(Path);
        base.OnDelete();
    }
}