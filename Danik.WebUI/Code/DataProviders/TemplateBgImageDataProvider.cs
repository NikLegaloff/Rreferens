using Danik.WebUI.Code.Domain;
using Danik.WebUI.Code.ORM;
using Danik.WebUI.Controllers;

namespace Danik.WebUI.Code.DataProviders;

public class TemplateBgImageDataProvider(Config config) : DataProvider<TemplateBgImage>(config)
{
    public TemplateBgImage Get(StoneType type, int? persons)
    {
        var all = SelectAll();
        foreach (var t in all)
        {
            if (type != StoneType.Сплит && t.Type == type) return t;
            if (type==StoneType.Сплит && t.Type == type && t.Persons == persons) return t;
        }
        throw new BusinessException("Unknown TemplateBgImage");
    }
}