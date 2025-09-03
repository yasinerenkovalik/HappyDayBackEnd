namespace HappyDay.Domain.Entities;

public class Category
{
    public int Id { get; set; } // int ID
    public string Name { get; set; }
    public ICollection<Organization> Organizations { get; set; }
}