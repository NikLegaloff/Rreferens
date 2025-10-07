using Danik.WebUI.Code.ORM;

namespace Danik.WebUI.Code.Domain;

public class StoneForm : ImageDomainObject, IComparable<StoneForm>
{
    public required int SortNumber { get; set; }
    public required StoneType Type { get; set; }
    public required int Persons { get; set; }

    public int CompareTo(StoneForm? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        return SortNumber.CompareTo(other.SortNumber);
    }
}