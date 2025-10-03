namespace HappyDay.Application.Features.Queries.Package.GetByCompany;

public class GetByOrganizastionPackageResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }          // Paket adı (örn: Gold, Silver, VIP)
    public string Description { get; set; }   // Açıklama
    public decimal Price { get; set; }        // Paket sabit fiyatı
    public decimal? PricePerPerson { get; set; } // Kişi başı fiyat (opsiyonel)
    public int? MaxGuests { get; set; } 
}