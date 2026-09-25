using Microsoft.EntityFrameworkCore;
using NutritionTrackingBot.Models;

namespace NutritionTrackingBot.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<FoodEntry> FoodEntries { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<FoodCatalog> FoodCatalogs { get; set; }

    public DbSet<UserFoodCatalog> UserFoodCatalogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserFoodCatalog>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<UserFoodCatalog>()
            .HasOne(x => x.FoodCatalog)
            .WithMany()
            .HasForeignKey(x => x.FoodCatalogId);

        modelBuilder.Entity<UserFoodCatalog>()
            .HasIndex(x => new { x.UserId, x.FoodCatalogId })
            .IsUnique();
        
        modelBuilder.Entity<FoodEntry>()
            .HasOne(x => x.FoodCatalog)
            .WithMany()
            .HasForeignKey(x => x.FoodCatalogId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FoodEntry>()
            .HasOne(x => x.user)
            .WithMany(x => x.FoodEntries)
            .HasForeignKey(x => x.userId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}