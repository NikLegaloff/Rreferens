using Danik.WebUI.Code.ORM;

namespace Danik.WebUI.Code.Domain;

public class SystemSettings : DomainObject
{
    public int OrdersCounter { get; set; } = 1000;
    public string Password { get; set; } = "123";
}