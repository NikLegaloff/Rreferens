using Danik.WebUI.Code.ORM;

namespace Danik.WebUI.Code.Domain;

public class User : DomainObject
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public bool IsAdmin { get; set; }
}