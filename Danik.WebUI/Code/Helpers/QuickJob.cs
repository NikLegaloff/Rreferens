using Danik.WebUI.Code.Domain;

namespace Danik.WebUI.Code.Helpers
{
    public class QuickJob
    {
        public void Process()
        {
            return;
            for (int i = 1; i <= 56; i++)
            {
                var path = "D:\\Dropbox\\Danik\\Tmp\\V-" + i +".jpg";
                var sf = new StoneForm(){Type = StoneType.Вертикальный,Persons = 1,SortNumber = i*10,ImageId = Image.Import(File.ReadAllBytes(path),"В 1ч №" + i+".jpg",ImageFolder.Формы_камней)};
                Registry.Current.StoneForms.Save(sf);
            }
           
            for (int i = 1; i <= 24; i++)
            {
                var path = "D:\\Dropbox\\Danik\\Tmp\\H-" + i +".jpg";
                var sf = new StoneForm(){Type = StoneType.Горизонтальный,Persons = 1,SortNumber = 1000 +i*10,ImageId = Image.Import(File.ReadAllBytes(path),"Г 1ч №" + i + ".jpg", ImageFolder.Формы_камней)};
                Registry.Current.StoneForms.Save(sf);
            }
           
        }

    }
}
