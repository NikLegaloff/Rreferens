using System.Data;
using Danik.WebUI.Code.DataProviders;
using Danik.WebUI.Code.Helpers;
using Danik.WebUI.Code.ORM;

namespace Danik.WebUI.Code.Domain
{
    public class Registry
    {
        public static Registry Current { get; } = new();

        public Registry()
        {
            var config = new Config(Env.Current.DataBasePath);
            Images = new DataProvider<Image>(config);
            StoneForms = new DataProvider<StoneForm>(config);
            Templates = new DataProvider<Template>(config);
            Orders = new DataProvider<Order>(config);
            SystemSettings = new SystemSettingsDataProvider(config);
        }

        public DataProvider<StoneForm> StoneForms { get; }
        public DataProvider<Image> Images { get; }
        public DataProvider<Order> Orders { get; }
        public SystemSettingsDataProvider SystemSettings { get; }
        public DataProvider<Template> Templates { get; set; }
    }
}
