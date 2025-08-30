using HappyDay.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HappyDay.Persistance.Context;

public class HappyDayContext : DbContext
{
    public HappyDayContext(DbContextOptions<HappyDayContext> options) : base(options) { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=aws-1-eu-north-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.bfnmewijysyhexybnkcn;Password=Eren1.2345;Ssl Mode=Require;Trust Server Certificate=true");
    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<OrganizationImage> OrganizationImages { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }
    public DbSet<CompanyInvitation> CompanyInvitations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Organization)
            .WithMany(o => o.Reservations)
            .HasForeignKey(r => r.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict); 
        modelBuilder.Entity<CompanyInvitation>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.TokenHash).IsUnique(); // Token tekil olsun
            b.Property(x => x.TokenHash).IsRequired().HasMaxLength(128);
            b.Property(x => x.Email).HasMaxLength(320);
            b.Property(x => x.CompanyNameHint).HasMaxLength(200);
        });

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