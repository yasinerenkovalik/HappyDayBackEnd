using HappyDay.Domain.Entities;
using HappyDay.Domain.Entities.BaseEntites;

public class Reservation : BaseEntity
{
    public DateTime ReservationDate { get; set; } // Tarih ve saat
    public int GuestCount { get; set; }
    public string Notes { get; set; }

    public Guid UserId { get; set; } // Customer

    public Guid CompanyId { get; set; }        // ✔ Foreign key
    public Company Company { get; set; }       // ✔ Navigation

    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; }
    public ReservationStatus Status { get; set; }
}
public enum ReservationStatus
{
    Pending,
    Approved,
    Rejected
}