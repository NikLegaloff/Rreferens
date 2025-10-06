using Danik.WebUI.Code.ORM;

namespace Danik.WebUI.Code.Domain;

public class StoneForm : ImageDomainObject
{
    public required int SortNumber { get; set; }
    public required StoneType Type { get; set; }
    public required int Persons { get; set; }
}