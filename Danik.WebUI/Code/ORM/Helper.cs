using Newtonsoft.Json;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Danik.WebUI.Code.ORM;

public static class Helper
{

    public static string ToJsonMin(this object subj) => JsonConvert.SerializeObject(subj, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore });
    public static Guid ToGuid(this string input) => new Guid(input);
    public static Guid MD5(this string input)
    {
#pragma warning disable SYSLIB0021
        var hasher = new MD5CryptoServiceProvider();
#pragma warning restore SYSLIB0021
        return new Guid(hasher.ComputeHash(Encoding.Default.GetBytes(input)));
    }

}