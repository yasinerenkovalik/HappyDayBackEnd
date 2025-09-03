using System.Globalization;
using CsvHelper;
using HappyDay.Domain.Entities;
using HappyDay.Persistance.Context;

namespace HappyDay.Api.Data;

public static class DataSeeder
{
    public static void SeedCitiesAndDistricts(HappyDayContext context)
    {
        // Eğer Districts boş değilse tekrar yükleme
        if (context.Districts.Any())
            return;

        using var reader = new StreamReader("Data/ilceler.csv");
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<IlceCsvRecord>().ToList();

        // Şehir sözlüğü: Province adı → City entity
        var cities = records
            .Select(r => r.Province)
            .Distinct()
            .Select(name => new City
            {
                CityName = name,
                Districts = new List<District>()
            })
            .ToDictionary(c => c.CityName);

        // İlçeleri oluştur ve District tablosuna ekle
        foreach (var r in records)
        {
            var district = new District
            {
                DistrictName = r.Name,
                City = cities[r.Province]
            };

            context.Districts.Add(district);
        }

        // Şehirleri ekle (ilçelerle beraber)
        context.Cities.AddRange(cities.Values);
        context.SaveChanges();
    }
}

public class IlceCsvRecord
{
    public required string Province { get; set; } // İl adı
    public required string Name { get; set; }     // İlçe adı
}