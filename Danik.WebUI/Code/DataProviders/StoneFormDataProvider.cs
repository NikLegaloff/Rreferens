using Danik.WebUI.Code.Domain;
using Danik.WebUI.Code.ORM;

namespace Danik.WebUI.Code.DataProviders;

public class GalleryImageDataProvider(Config config) : DataProvider<GalleryImage>(config)
{
    public GalleryImage[] SelectByPartnerId(Guid partnerId)
    {
        var all = SelectAll();
        var filtered = all.Where(img => img.PartnerId == partnerId).ToArray();
        Array.Sort(filtered, (a, b) => a.Sort.CompareTo(b.Sort));
        return filtered;
    }
}
public class StoneFormDataProvider(Config config) : DataProvider<StoneForm>(config)
{

}