using HappyDay.Domain.Entities.BaseEntites;

namespace HappyDay.Domain.Entities;

public class OrganizationImage:BaseEntity
{
    public int Id { get; set; }
    public string ImageUrl { get; set; }

    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; }
}