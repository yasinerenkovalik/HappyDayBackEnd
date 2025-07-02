using HappyDay.Domain.Entities.BaseEntites;

namespace HappyDay.Domain.Entities
{
    public class City : BaseEntity
    {
        public int Id { get; set; } 
        public string CityName { get; set; }

        // Navigation
    
        public ICollection<District> Districts { get; set; }
        public ICollection<Organization> Organizations { get; set; }
    }
}