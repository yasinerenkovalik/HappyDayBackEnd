using System.Text.Json.Serialization;
using HappyDay.Domain.Entities.BaseEntites;

namespace HappyDay.Domain.Entities;

public class Company:BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    [JsonIgnore] public string PasswordHash { get; set; } = default!;
    public string Adress { get; set; }
    public string PhoneNumber { get; set; }
    public string Description { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    
    public ICollection<Organization> Organizations { get; set; }
    public bool IsApproved { get; set; } // Company
}