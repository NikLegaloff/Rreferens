using Danik.WebUI.Code.Domain;

namespace Danik.WebUI.Code.Helpers
{
    public class QuickJob
    {
        public void Process()
        {
            Download("https://stella-pamyat.ru/assets/img/constructor/flowers/cvetok8.png", "Цветок 8.png");
        }

        private static void Download(string url, string name)
        {
            using var webClient = new System.Net.WebClient();
            var data = webClient.DownloadData(url);
            var imgId = Image.Import(data, name, ImageFolder.Клипарт_гориз);
        }
    }
}
