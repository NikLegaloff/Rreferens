using Danik.WebUI.Code.ORM;

namespace Danik.WebUI.Code.Domain;

public class Epitaph : DomainObject, IComparable<Epitaph>
{
    public  int? Persons{ get; set; }
    public  required string Text{ get; set; }

    public int CompareTo(Epitaph? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        var personsComparison = Nullable.Compare(Persons, other.Persons);
        if (personsComparison != 0) return personsComparison;
        return string.Compare(Text, other.Text, StringComparison.Ordinal);
    }
}