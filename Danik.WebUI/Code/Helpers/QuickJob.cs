namespace Danik.WebUI.Code.Helpers
{
    public class QuickJob
    {
        public void Process()
        {

           // Download("p", 56, "V");
            Download("w", 45, "H");
        }

        private void Download(string letter, int count, string newName)
        {
            for (int i = 1; i <= count; i++)
            {
                var url = $"https://stella-pamyat.ru/assets/img/constructor/memorial-dvoynoy/{letter}{i}.png";
                var path = $@"d:\Dropbox\Danik\Tmp\{newName}-{i}.jpg";
                using var client = new HttpClient();
                var data = client.GetByteArrayAsync(url).Result;
                File.WriteAllBytes(path, data);
                Console.WriteLine(letter + i);
            }
        }
    }
}
