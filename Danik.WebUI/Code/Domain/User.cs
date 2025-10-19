using Danik.WebUI.Code.ORM;

namespace Danik.WebUI.Code.Domain;

public class Partner : DomainObject
{
    public required string Alias { get; set; }
    public required string Name { get; set; }

}
public class User : DomainObject
{
    public Guid PartnerId{ get; set; }
    public required string Email { get; set; }
    public required Guid Password { get; set; }
    public required string Name { get; set; }
    public required bool IsAdmin { get; set; }
}