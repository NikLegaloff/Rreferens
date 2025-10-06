using Danik.WebUI.Code.ORM;

namespace Danik.WebUI.Code.Domain;

public class TemplateBgImage : ImageDomainObject
{
    public StoneType Type { get; set; }
    public int? Persons { get; set; }
}