namespace Danik.WebUI.Code.Domain;

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