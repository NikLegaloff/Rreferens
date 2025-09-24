using Danik.WebUI.Code.ORM;

namespace Danik.WebUI.Code.Domain;

public class Template : DomainObject
{
    public bool IsVert { get; set; }
    public Guid ImageId { get; set; }
    
    public required TemplatePortrait[] Portraits { get; set; }
    public required TemplateImage[] Images { get; set; }
    public required TemplateText[] Texts { get; set; }
}

public class TemplatePortrait
{
    public required string URL { get; set; }
    public required Area Area { get; set; }
}

public class TemplateImage
{
    public required string URL{ get; set; }
    public bool IsVert{ get; set; }
    public required Area Area { get; set; }
}

public class TemplateText
{
    public required string Text { get; set; }
    public required Area Area { get; set; }
    public Align Align { get; set; }
    public bool Bold{ get; set; }
    public int Size{ get; set; }
    public string? Font{ get; set; }
}

public enum Align { Center, Left, Right }

public class Area
{
    public int X { get; set; }
    public int Y { get; set; }
    public int W { get; set; }
    public int? H { get; set; }
}
