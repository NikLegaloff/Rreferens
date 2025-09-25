using Danik.WebUI.Code.ORM;
using Newtonsoft.Json;

namespace Danik.WebUI.Code.Domain;

public class Order : DomainObject
{
    public DateTime Date{ get; set; }
    public required string Number{ get; set; }
    public required int Persons{ get; set; }
    public string? Phone{ get; set; }
    public string? Email{ get; set; }
    public string? ContactName{ get; set; }
    public string? Comment{ get; set; }

    public Guid? TemplateId{ get; set; }
    public Guid[]? PortraitImages{ get; set; }
    public PersonInfo[]? PersonInfos{ get; set; }

    public Guid[]? ExampleImages{ get; set; }

    public OrderOptions Options { get; set; } = new();
}


public class PersonInfo
{
    public Guid ImageId{ get; set; }
    public string F{ get; set; }
    public string I{ get; set; }
    public string O{ get; set; }
    public string Comment{ get; set; }
    public string Birth{ get; set; }
    public string Dead{ get; set; }
    public string Epitaph { get; set; }

}
public class OrderOptions
{
    public bool IsVert { get; set; }
    public int Size { get; set; }
    public string? Depth { get; set; }
    public string? OwnSize { get; set; }

    [JsonIgnore]
    public string SizeStr
    {
        get
        {
            if (!string.IsNullOrEmpty(OwnSize)) return OwnSize;
            if (Size == 40) return "40x80";
            if (Size == 50) return "50x100";
            if (Size == 60) return "60x120";
            return Size.ToString();
        }
    }

}