using System.Data;
using Danik.WebUI.Code.ORM;

namespace Danik.WebUI.Code.Domain
{
    public class Registry
    {
        public static Registry Current { get; } = new();

        public Registry()
        {
            var config = new Config(Env.Current.DataBasePath);
            Images = new DomainTable<Image>(config);
            Templates = new DomainTable<Template>(config);
            Orders = new DomainTable<Order>(config);
        }

        public DomainTable<Image> Images { get; }
        public DomainTable<Order> Orders { get; }
        public DomainTable<Template> Templates { get; set; }
    }
}
