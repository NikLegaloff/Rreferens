using Danik.WebUI.Code.Domain;

namespace Danik.WebUI.Models;

public record WizStep2(Order Order, Image Image);
public record WizStep4(Order Order, Template Template);
public record OrdersList(Order[] Orders, OrderStatus? Status, string? Q, Guid? PartnerId);
public record GalleryList(Image[] Images, ImageFolder Folder);