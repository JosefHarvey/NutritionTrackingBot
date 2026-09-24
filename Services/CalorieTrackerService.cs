using NutritionTrackingBot.Models;
using NutritionTrackingBot.Data;
using Microsoft.EntityFrameworkCore;
using NutritionTrackingBot.Helpers;

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

    public async Task<FoodEntryResponse?> AddFoodAsync(FoodEntry food, int userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return null;
        }

        food.userId = userId;

        _context.FoodEntries.Add(food);

        await _context.SaveChangesAsync();

        return new FoodEntryResponse
        {
            Id = food.Id,
            FoodName = food.FoodName,
            Calories = food.Calories,
            Protein = food.Protein,
            Fat = food.Fat,
            Carbs = food.Carbs,
            ConsumedAt = food.ConsumedAt
        };
    }

    public async Task<DailyNutritionSummary?> GetDailySummaryAsync(int userId)
    {   
        //get users
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return null;
        }

        var (startUtc, endUtc) = TimezoneHelper.GetUtcRangeForToday(user.TimeZoneId);

        var foods = await _context.FoodEntries
        .Where(f => f.ConsumedAt >= startUtc && f.ConsumedAt < endUtc && f.userId == userId)
        .ToListAsync();

        return new DailyNutritionSummary
        {
            Calories = foods.Sum(f => f.Calories),
            Protein = foods.Sum(f => f.Protein),
            Fat = foods.Sum(f => f.Fat),
            Carbs = foods.Sum(f => f.Carbs)
        };
    }

    public async Task<bool> ResetDailyTrackingAsync(int userId)
    {
        //Get User
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return false;
        }

        // Get today's UTC range based on user's timezone
        var (startUtc, endUtc) =
        TimezoneHelper.GetUtcRangeForToday(user.TimeZoneId);

        var foods = await _context.FoodEntries.Where(f =>
            f.userId == userId &&
            f.ConsumedAt >= startUtc &&
            f.ConsumedAt < endUtc)
        .ToListAsync();

        _context.FoodEntries.RemoveRange(foods);

        await _context.SaveChangesAsync();

        return true;
    }
    public async Task<List<FoodEntryResponse>?> GetHistoryAsync(int userId, DateTime date)
    {
        //get users
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return null;
        }

        //get UTC range for the specified local date based on the user's timezone
       var(startUtc,endUtc) = TimezoneHelper.GetUtcRangeForLocalDate(user.TimeZoneId,date);

        return await _context.FoodEntries
        .Where(f => f.userId == userId && f.ConsumedAt >= startUtc && f.ConsumedAt < endUtc)
        .OrderBy(f => f.ConsumedAt)
        .Select(f => new FoodEntryResponse
        {
            Id = f.Id,
            FoodName = f.FoodName,
            Calories = f.Calories,
            Protein = f.Protein,
            Fat = f.Fat,
            Carbs = f.Carbs,
            ConsumedAt = f.ConsumedAt
        })
        .ToListAsync();
    }
    public async Task<FoodEntryResponse?> GetFoodByIdAsync(int id)
    {
        return await _context.FoodEntries.Where(f => f.Id == id)
        .Select(f => new FoodEntryResponse
        {
            Id = f.Id,
            FoodName = f.FoodName,
            Calories = f.Calories,
            Protein = f.Protein,
            Fat = f.Fat,
            Carbs = f.Carbs,
            ConsumedAt = f.ConsumedAt
        })
        .FirstOrDefaultAsync();
    }
}