using Danik.WebUI.Code.Helpers;
using Danik.WebUI.Code.ORM;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Danik.WebUI.Code.Domain;

public class GalleryImage : DomainObject, IComparable<GalleryImage>
{
    public static string GetURL(Guid id) => $"/Images/GetGalleryImage/{id}";
    public static string GetTmb(Guid id) => $"/Images/GetGalleryTmb/{id}";

    [JsonIgnore]
    public string URL => GetURL(Id);
    [JsonIgnore]
    public string TMB => GetTmb(Id);

    [JsonIgnore]
    //public string Path => $"{Env.Current.DataBasePath}Images\\{Id}." + Ext;
    public  string Path => $"{Env.Current.DataBasePath}GalleryImages\\{Alias}\\{Id}.jpg";
    [JsonIgnore]
    public string TmbPath => $"{Env.Current.DataBasePath}GalleryImages\\{Alias}\\{Id}_tmb.jpg";

    public required int Sort { get; set; }
    public required string Alias { get; set; }
    public required Guid PartnerId { get; set; }



    public static Guid Import(byte[] data, Guid partnerId)
    {
        var partner = Registry.Current.Partners.Find(partnerId);
        if (partner == null) throw new Exception("Partner not found");
        var alias = partner.Alias;

        var img = new GalleryImage
        {
            Alias = alias,
            PartnerId = partnerId,
            Sort = Registry.Current.GalleryImages.SelectAll().Count(i => i.PartnerId == partnerId) + 1
        };
        Registry.Current.GalleryImages.Save(img);

        var dir = System.IO.Path.GetDirectoryName(img.Path);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir!);
        
        var main = SixLabors.ImageSharp.Image.Load(data).Clone(ctx => ctx.Resize(new ResizeOptions
        {
            Size = new Size(1200, 1200),
            Mode = ResizeMode.Max
        }));
        main.SaveAsJpeg(img.Path);

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

    public int CompareTo(GalleryImage? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return Sort.CompareTo(other.Sort);
    }
}