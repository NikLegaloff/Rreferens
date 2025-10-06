using Danik.WebUI.Code.Domain;

namespace Danik.WebUI.Code.ORM;

public class ImageDomainObject : DomainObject
{
    public Guid ImageId { get; set; }
    public string ImageURL => Image.GetURL(ImageId);
    public string ImageTMB => Image.GetTmb(ImageId);

    public override void OnDelete()
    {
        Registry.Current.Images.Delete(ImageId);
    }
}