using Danik.WebUI.Code.ORM;
using Newtonsoft.Json;

namespace Danik.WebUI.Code.Domain;

public class Template : DomainObject, IComparable<Template>
{
    public StoneType Type{ get; set; }
    public int Persons{ get; set; }
    public TemplateData Data { get; set; }

    public int CompareTo(Template? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        var typeComparison = Type.CompareTo(other.Type);
        if (typeComparison != 0) return typeComparison;
        return Persons.CompareTo(other.Persons);
    }
}

public class TemplateData
{
    public bool SingleEpitaph
    {
        get { return Texts.Any(t => t.Text.Contains("{эпитафия}")); }
    }

    public Guid BgImageId { get; set; }
    public required TemplatePortrait[] Portraits { get; set; }
    public TemplateImage[]? Images { get; set; }
    public required TemplateText[] Texts { get; set; }
}
public class TemplateLayer
{
    public int Num{ get; set; }
    public required Area Area { get; set; }

}
public class TemplatePortrait : TemplateLayer
{
    public required string ImageId { get; set; }
}

public class TemplateImage: TemplateLayer
{
    public required string ImageId{ get; set; }
    public bool IsVert{ get; set; }
}


public class TemplateText: TemplateLayer
{
    public required string Text { get; set; }
    public Align Align { get; set; }=Align.Center;
    public string AlignStr => Align.ToString().ToLower();
    public bool Bold{ get; set; }
    public int Size{ get; set; }
    public string? Font{ get; set; }
}

public enum Align { Center, Left, Right }

public class Area
{
    public Area()
    {
    }

    public Area(int x, int y, int w, int? h=null)
    {
        X = x;
        Y = y;
        W = w;
        H = h;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public int W { get; set; }
    public int? H { get; set; }
}
