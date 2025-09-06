using HappyDay.Domain.Entities.BaseEntites;

namespace HappyDay.Domain.Entities;

public class Contact:BaseEntity
{
    public string Name { get; set; }
    public string SurName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Mesaage { get; set; }
}