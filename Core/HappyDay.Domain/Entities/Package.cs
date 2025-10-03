using HappyDay.Domain.Entities;
using HappyDay.Domain.Entities.BaseEntites;

public class Package:BaseEntity
{
   
    public string Name { get; set; }          // Paket adı (örn: Gold, Silver, VIP)
    public string Description { get; set; }   // Açıklama
    public decimal Price { get; set; }        // Paket sabit fiyatı
    public decimal? PricePerPerson { get; set; } // Kişi başı fiyat (opsiyonel)
    public int? MaxGuests { get; set; }       // Maksimum kişi sayısı

    // Foreign Key
    public Guid OrganizationId { get; set; }

    // Navigation Property
    public Organization Organization { get; set; }
}