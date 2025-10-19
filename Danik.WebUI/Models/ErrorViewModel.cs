using Danik.WebUI.Code.Domain;

namespace Danik.WebUI.Models
{
    public record StartPageModel(GalleryImage[] Images);

    
        
    

    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
