using HappyDay.Domain.Entities.BaseEntites;

namespace HappyDay.Domain.Entities;

public class Company:BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Adress { get; set; }
    public string PhoneNumber { get; set; }
    public string Description { get; set; }
    
    public ICollection<Organization> Organizations { get; set; }
    public bool IsApproved { get; set; } // Company
}