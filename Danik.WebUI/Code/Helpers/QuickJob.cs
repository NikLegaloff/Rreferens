using Danik.WebUI.Code.Domain;

namespace Danik.WebUI.Code.Helpers
{
    public class QuickJob
    {
        public void Process()
        {
            for (int i = 1; i < 52; i++)
            {
                var url = "https://stella-pamyat.ru/assets/img/constructor/candles/svecha" + i + ".png";
                // download bytes from url
                using var webClient = new System.Net.WebClient();
                var data = webClient.DownloadData(url);
                var imgId = Image.Import(data, "Свеча " + i + ".png", ImageFolder.Клипарт);
                Console.WriteLine(i);
            }

        }

    }
}
