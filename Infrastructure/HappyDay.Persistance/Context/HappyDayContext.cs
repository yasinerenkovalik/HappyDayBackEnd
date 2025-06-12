using HappyDay.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Persistance.Context;

public class HappyDayContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Server=localhost;Database=HappyDay;User Id=admin;Password=123123;TrustServerCertificate=True;");
    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
}
