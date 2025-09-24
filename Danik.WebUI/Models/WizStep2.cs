using Danik.WebUI.Code.Domain;

namespace Danik.WebUI.Models;

public record WizStep2(Order Order, Guid ImageId, string ImageUrl);