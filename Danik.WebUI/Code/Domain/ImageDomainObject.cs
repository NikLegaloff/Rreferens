using Danik.WebUI.Code.ORM;

namespace Danik.WebUI.Code.Domain;

public class ImageDomainObject : DomainObject
{
    public Guid ImageId { get; set; }

    public override void OnDelete()
    {
        Registry.Current.Images.Delete(ImageId);
    }
}