using Danik.WebUI.Code.ORM;

namespace Danik.WebUI.Code.Domain;

public enum Lang{Ru,Ro}
public class Epitaph : DomainObject, IComparable<Epitaph>
{
    public required Lang Lang { get; set; }
    public  int? Persons{ get; set; }
    public  required string Text{ get; set; }

    public int CompareTo(Epitaph? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        var res = Lang.CompareTo(other.Lang);
        if (res != 0) return res;
         res = Nullable.Compare(Persons, other.Persons);
        if (res != 0) return res;
        return string.Compare(Text, other.Text, StringComparison.Ordinal);
    }

    public bool IsOkFor(Order order)
    {
        if (order.Lang != Lang) return false;
        if (Persons == null) return true;
        return order.Persons == 1 ? Persons.Value == 1 : Persons.Value > 1;
    }
}