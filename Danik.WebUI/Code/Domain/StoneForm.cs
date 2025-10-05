namespace Danik.WebUI.Code.Domain;

public class StoneForm : ImageDomainObject
{
    public int SortNumber { get; set; }
    public required Type Type { get; set; }
    public int Persons { get; set; }
}