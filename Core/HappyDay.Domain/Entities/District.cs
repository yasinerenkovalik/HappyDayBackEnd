using System.ComponentModel.DataAnnotations.Schema;
using HappyDay.Domain.Entities.BaseEntites;

namespace HappyDay.Domain.Entities
{
    public class District 
    {
      
        public int Id { get; set; } 
        public string DistrictName { get; set; }

        // Foreign key
        public int CityId { get; set; }
        public City City { get; set; }

        // Navigation
        public ICollection<Organization> Organizations { get; set; }
    }
}