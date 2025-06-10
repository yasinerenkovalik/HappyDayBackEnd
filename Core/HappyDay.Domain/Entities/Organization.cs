using HappyDay.Domain.Entities.BaseEntites;

namespace HappyDay.Domain.Entities;

public class Organization:BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int MaxGuestCount { get; set; }

    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
    public ICollection<Reservation> Reservations { get; set; }

}