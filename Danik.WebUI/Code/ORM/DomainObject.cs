using Newtonsoft.Json;

namespace Danik.WebUI.Code.ORM;

public class DomainObject
{
    [JsonIgnore]
    public Guid Id { get; set; }

    public virtual void OnDelete() { }
}