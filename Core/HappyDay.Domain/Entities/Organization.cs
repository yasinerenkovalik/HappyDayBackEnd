using HappyDay.Domain.Entities.BaseEntites;

namespace HappyDay.Domain.Entities;

public class Organization:BaseEntity
{
    public string Title { get; set; }
    public int CategoryId { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int MaxGuestCount { get; set; }
    public string Location { get; set; }
    public int CityId { get; set; }
    public City City { get; set; }

    public int DistrictId { get; set; }
    public District District { get; set; }
    public List<string> Services { get; set; } = new();
    public string Duration { get; set; }
    public bool IsOutdoor { get; set; }
    public string ReservationNote { get; set; }
    public string CancelPolicy { get; set; }
    public string VideoUrl { get; set; }
    
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
    public string? CoverPhotoPath { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
    public ICollection<OrganizationImage> OrganizationImages { get; set; }


}