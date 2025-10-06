using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace Danik.WebUI.Code.Helpers;

public class Env
{
    public static Env Current { get; } = new();
    public enum Type { Dev,Live }
    public Type EnvType => Type.Dev;

    public string DataBasePath
    {
        get
        {
            if (EnvType== Type.Dev) return "D:\\Dropbox\\Danik\\";
            return "";
        }
    }
}

public static class EncryptionHelper
{

    public static string MD5(string input)
    {
#pragma warning disable SYSLIB0021
        var hasher = new MD5CryptoServiceProvider();
#pragma warning restore SYSLIB0021
        var data = hasher.ComputeHash(Encoding.Default.GetBytes(input));
        var builder = new StringBuilder();
        foreach (var t in data) builder.Append(t.ToString("x2"));
        return builder.ToString();
    }

    public static Guid MD5Guid(byte[] input)
    {
        using MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] hash = md5.ComputeHash(input);
        return new Guid(hash);
    }


    public static Guid MD5Guid(string input)
    {
#pragma warning disable SYSLIB0021
        var hasher = new MD5CryptoServiceProvider();
#pragma warning restore SYSLIB0021
        return new Guid(hasher.ComputeHash(Encoding.Default.GetBytes(input)));
    }


#pragma warning restore SYSLIB0011
}