using Danik.WebUI.Code.ORM;
using Newtonsoft.Json;

namespace Danik.WebUI.Code.Domain;

public enum StoneType
{
    Вертикальный, Горизонтальный, Сплит
}
public enum OrderStatus
{
    Создаётся,
    Создан,
    Оплачен,
    Завершён,
    Отменён,
}

public class Order : DomainObject
{
    public Guid? UserId{ get; set; }

    public required string Number{ get; set; }
    public OrderStatus Status { get; set; }=OrderStatus.Создаётся;
    public DateTime Date{ get; set; }

    public required int Persons{ get; set; }
    public required StoneType Type{ get; set; }

    public Guid? StoneForm { get; set; }
    public Guid? StoreFormImage { get; set; }

    public Contact? Contact { get; set; }
    public string? Comment { get; set; }
    

    public Guid? TemplateId{ get; set; }

    public Guid[]? PortraitImages{ get; set; }

    public PersonInfo[]? PersonInfos{ get; set; }
    public string? Epitaph { get; set; }
    public TemplateData? TemplateData { get; set; }



    public Guid[]? ExampleImages { get; set; }
    public OrderOptions Options { get; set; } = new();
}


public class Contact
{
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public string? Email { get; set; }
}
public class PersonInfo
{
    public Guid ImageId{ get; set; }
    public string F{ get; set; }
    public string I{ get; set; }
    public string O{ get; set; }
    
    public string Birth{ get; set; }
    public string Dead{ get; set; }

    public string Comment { get; set; }

}
public class OrderOptions
{

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