using System.Data;
using Danik.WebUI.Code.DataProviders;
using Danik.WebUI.Code.Domain;
using Danik.WebUI.Code.Helpers;
using Danik.WebUI.Code.ORM;

namespace Danik.WebUI.Code
{
    public class Registry
    {
        public static Registry Current { get; } = new();

        public Registry()
        {
            var config = new Config(Env.Current.DataBasePath);
            Images = new DataProvider<Image>(config);
            Templates = new TemplateDataProvider(config);
            Users = new DataProvider<User>(config);
            Orders = new DataProvider<Order>(config);
            Epitaphs = new DataProvider<Epitaph>(config);
            TemplateBgImages = new TemplateBgImageDataProvider(config);

            StoneForms = new StoneFormDataProvider(config);
            SystemSettings = new SystemSettingsDataProvider(config);
        }

        public DataProvider<Image> Images { get; }
        public DataProvider<Order> Orders { get; }
        public DataProvider<Epitaph> Epitaphs { get; }
        public TemplateDataProvider Templates { get; set; }
        public DataProvider<User> Users { get; set; }
        public TemplateBgImageDataProvider TemplateBgImages { get; set; }

        public StoneFormDataProvider StoneForms { get; }
        public SystemSettingsDataProvider SystemSettings { get; }
    }
}
