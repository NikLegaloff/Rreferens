using Danik.WebUI.Code.Domain;

namespace Danik.WebUI.Code.Helpers
{
    public class QuickJob
    {
        public void Process()
        {
            return;
            string path;
            path = "J:\\Projects\\Danik\\Danik.WebUI\\wwwroot\\img\\TV.png";
            var sf = new TemplateBgImage() { Type = StoneType.Вертикальный, ImageId = Image.Import(File.ReadAllBytes(path), "Вертикаль.png", ImageFolder.Фоны_шаблонов) };
            Registry.Current.TemplateBgImages.Save(sf);

            path = "J:\\Projects\\Danik\\Danik.WebUI\\wwwroot\\img\\TH.png";
            sf = new TemplateBgImage() { Type = StoneType.Горизонтальный, ImageId = Image.Import(File.ReadAllBytes(path), "Горизонталь.png", ImageFolder.Фоны_шаблонов) };
            Registry.Current.TemplateBgImages.Save(sf);

            path = "J:\\Projects\\Danik\\Danik.WebUI\\wwwroot\\img\\TS2.png";
            sf = new TemplateBgImage() { Type = StoneType.Сплит,Persons = 2,ImageId = Image.Import(File.ReadAllBytes(path), "Сплит 2.pmg", ImageFolder.Фоны_шаблонов) };
            Registry.Current.TemplateBgImages.Save(sf);

            path = "J:\\Projects\\Danik\\Danik.WebUI\\wwwroot\\img\\TS3.png";
            sf = new TemplateBgImage() { Type = StoneType.Сплит,Persons = 3,ImageId = Image.Import(File.ReadAllBytes(path), "Сплит 3.pmg", ImageFolder.Фоны_шаблонов) };
            Registry.Current.TemplateBgImages.Save(sf);



        }

    }
}
