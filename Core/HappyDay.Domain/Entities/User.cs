using System.Text.Json.Serialization;
using HappyDay.Domain.Entities.BaseEntites;

namespace HappyDay.Domain.Entities;

public class User:BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    [JsonIgnore] public string PasswordHash { get; set; } = default!;
    public string SurName { get; set; }
    public DateTime BirtDay { get; set; }
    public int IdentityNo { get; set; }
    public string Adress { get; set; }
    public string PhoneNumber { get; set; }
    public bool Sex { get; set; }
    
}
