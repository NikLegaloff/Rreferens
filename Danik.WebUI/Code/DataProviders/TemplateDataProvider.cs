using Danik.WebUI.Code.Domain;
using Danik.WebUI.Code.ORM;

namespace Danik.WebUI.Code.DataProviders;

public class TemplateDataProvider(Config config) : DataProvider<Template>(config)
{
    public Template[] SelectFor(Order order)
    {
        return SelectAll().Where(t => t.Type == order.Type && t.Persons == order.Persons).ToArray();
    }

}