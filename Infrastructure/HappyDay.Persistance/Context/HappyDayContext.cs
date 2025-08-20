using HappyDay.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Persistance.Context;

public class HappyDayContext : DbContext
{
    private readonly string _connectionString;

    public HappyDayContext()
    {
        // Default connection string for development
        _connectionString = "Host=127.0.0.1;Database=happydaydb;Username=postgres;Password=postgres;TrustServerCertificate=True;";
    }

    public HappyDayContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public HappyDayContext(DbContextOptions<HappyDayContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }
    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<OrganizationImage> OrganizationImages { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<District> Districts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Organization -> City
        modelBuilder.Entity<Organization>()
            .HasOne(o => o.City)
            .WithMany(c => c.Organizations)
            .HasForeignKey(o => o.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        // Organization -> District
        modelBuilder.Entity<Organization>()
            .HasOne(o => o.District)
            .WithMany(d => d.Organizations)
            .HasForeignKey(o => o.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);

        // District -> City
        modelBuilder.Entity<District>()
            .HasOne(d => d.City)
            .WithMany(c => c.Districts)
            .HasForeignKey(d => d.CityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}