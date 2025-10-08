using Newtonsoft.Json;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Danik.WebUI.Code.ORM;

public static class Helper
{

    public static string ToJsonMin(this object subj) => JsonConvert.SerializeObject(subj, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });
    public static string ToJsonMax(this object subj) => JsonConvert.SerializeObject(subj, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Include });
    public static Guid ToGuid(this string input) => new Guid(input);
    public static Guid MD5(this string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return Guid.Empty;
#pragma warning disable SYSLIB0021
        var hasher = new MD5CryptoServiceProvider();
#pragma warning restore SYSLIB0021
        return new Guid(hasher.ComputeHash(Encoding.Default.GetBytes(input)));
    }

    public static T? ToSubj<T>(this string json)
    {
        return JsonConvert.DeserializeObject<T>(json);
    }


    public static string? ToMinJSON<T>(this T? subj, bool typeNameHandle = false, bool ignoreDefaults = true)
    {
        if (subj == null) return null;
        var settings = new JsonSerializerSettings();
        if (ignoreDefaults)
        {
            settings.DefaultValueHandling = DefaultValueHandling.Ignore;
            settings.NullValueHandling = NullValueHandling.Ignore;
        }
        if (typeNameHandle) settings.TypeNameHandling = TypeNameHandling.Objects;
        var minJSON = JsonConvert.SerializeObject(subj, Formatting.None, settings);
        return minJSON;
    }


}