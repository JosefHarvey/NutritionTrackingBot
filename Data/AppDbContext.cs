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
}