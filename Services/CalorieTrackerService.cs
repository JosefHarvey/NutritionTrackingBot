using NutritionTrackingBot.Models;
using NutritionTrackingBot.Data;
using Microsoft.EntityFrameworkCore;

namespace NutritionTrackingBot.Services;

public class CalorieTrackerService : ICalorieTrackerService
{
    // Local storage 
    // private readonly List<FoodEntry> _foods = new();

    // Database storage
    private readonly AppDbContext _context;
    public CalorieTrackerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddFoodAsync(FoodEntry food)
    {
        _context.FoodEntries.Add(food);
        await _context.SaveChangesAsync();
    }

    public async Task<DailyNutritionSummary> GetDailySummaryAsync()
    {   
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var foods = await _context.FoodEntries
        .Where(f => f.ConsumedAt >= today && f.ConsumedAt < tomorrow)
        .ToListAsync();

        return new DailyNutritionSummary
        {
            Calories = foods.Sum(f => f.Calories),
            Protein = foods.Sum(f => f.Protein),
            Fat = foods.Sum(f => f.Fat),
            Carbs = foods.Sum(f => f.Carbs)
        };
    }

    public async Task ResetDailyTrackingAsync()
    {
       var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var foods = await _context.FoodEntries
            .Where(f => f.ConsumedAt >= today && f.ConsumedAt < tomorrow)
            .ToListAsync();

        _context.FoodEntries.RemoveRange(foods);

        await _context.SaveChangesAsync();
    }
}