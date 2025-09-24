using Danik.WebUI.Code.ORM;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Danik.WebUI.Code.Domain;

public class Image : DomainObject
{
    public required DateTime Date { get; set; }
    public required string Name { get; set; }
    
    [JsonIgnore]
    public string URL => $"/Images/GetImage/{Id}";
    [JsonIgnore]
    public string TMB => $"/Images/GetTmb/{Id}";
    
    [JsonIgnore]
    public string Path => $"{Env.Current.DataBasePath}Images\\{Id}." + Ext;

    public string Ext => Name.Contains(".") ? Name.Split('.').Last():"jpg";

    [JsonIgnore]
    public string TmbPath => $"{Env.Current.DataBasePath}Images\\{Id}_tmb.jpg";

    public void SaveImageData(byte[] data)
    {
        var dir = System.IO.Path.GetDirectoryName(Path);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir!);
        File.WriteAllBytes(Path, data);
    }

    public static Guid Import(byte[] data, string name)
    {
        var img = new Image() { Date = DateTime.Now, Name = name };
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