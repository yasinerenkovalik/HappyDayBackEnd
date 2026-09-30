using HappyDay.Domain.Entities.BaseEntites;

namespace HappyDay.Domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string CityName { get; set; } = default!;

        /// <summary>Konum tespiti için il merkezinin koordinatı (yaklaşık).</summary>
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        // Navigation
        public ICollection<District> Districts { get; set; } = new List<District>();
        public ICollection<Organization> Organizations { get; set; } = new List<Organization>();
    }
}
