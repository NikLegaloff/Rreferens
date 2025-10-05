using Danik.WebUI.Code.Domain;
using Danik.WebUI.Code.ORM;

namespace Danik.WebUI.Code.DataProviders;

public class SystemSettingsDataProvider(Config config) : DataProvider<SystemSettings>(config)
{
    public SystemSettings Current()
    {
        var all = SelectAll();
        if (all.Length != 0) return all[0];
        var sys = new SystemSettings();
        Save(sys);
        return sys;
    }
    public string GetOrderNumber()
    {
        var c = Current();
        var res = c.OrdersCounter.ToString();
        c.OrdersCounter++;
        Save(c);
        return res;
    }
}