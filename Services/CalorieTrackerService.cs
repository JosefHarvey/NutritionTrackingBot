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

    public async Task<DailyNutritionSummary> GetDailySummaryAsync(int userId)
    {   
        //get users
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if(user == null)
        {
            throw new Exception("User not found");
        }

        //get user timezone
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId);

        var localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,timeZone);

        var localStart = DateTime.SpecifyKind(localNow.Date,DateTimeKind.Unspecified);

        var localEnd = localStart.AddDays(1);

        //convert to UTC
        var startUtc = TimeZoneInfo.ConvertTimeToUtc(localStart, timeZone);
        var endUtc = TimeZoneInfo.ConvertTimeToUtc(localEnd, timeZone);

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
    public async Task<List<FoodEntryResponse>>GetHistoryAsync(int userId, DateTime date)
    {
        //get users
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if(user == null)
        {
            throw new Exception("User not found");
        }

        //get user timezone
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZoneId);

        var localDate = DateTime.SpecifyKind(date.Date, DateTimeKind.Unspecified);

        var localStart = localDate;
        var localEnd = localStart.AddDays(1);

        //convert to UTC
        var startUtc = TimeZoneInfo.ConvertTimeToUtc(localStart, timeZone);
        var endUtc = TimeZoneInfo.ConvertTimeToUtc(localEnd, timeZone);

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
}