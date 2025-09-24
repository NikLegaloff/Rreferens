namespace Danik.WebUI.Code.ORM;

public class Config(string basePath)
{
    public string BasePath { get; private set; } = basePath;
}