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

    public async Task<AddFoodResult> AddFoodAsync(AddFoodRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);

        if (user == null)
        {
            return new AddFoodResult
            {
                Status = AddFoodStatus.UserNotFound
            };
        }

        var foodCatalog = await _context.FoodCatalogs.FirstOrDefaultAsync(f => f.Id == request.FoodCatalogId);

        if (foodCatalog == null)
        {
            return new AddFoodResult
            {
                Status = AddFoodStatus.FoodCatalogNotFound
            };
        }

        var userFoodCatalog = await _context.UserFoodCatalogs.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.FoodCatalogId == request.FoodCatalogId);

        double calories;
        double protein;
        double fat;
        double carbs;

        if(userFoodCatalog != null)
        {
            // Use the user's custom values
            calories = userFoodCatalog.Calories;
            protein = userFoodCatalog.Protein;
            fat = userFoodCatalog.Fat;
            carbs = userFoodCatalog.Carbs;
        }
        else
        {
            // Use the default values from the food catalog
            calories = foodCatalog.Calories;
            protein = foodCatalog.Protein;
            fat = foodCatalog.Fat;
            carbs = foodCatalog.Carbs;
        }

        //check if the unit matches the food catalog's serving unit
        if(request.Unit != foodCatalog.ServingUnit)
        {
           return new AddFoodResult
            {
                Status = AddFoodStatus.InvalidUnit
            };
        }

        // Calculate the multiplier based on the requested quantity and the serving size from the food catalog
        var multiplier = request.Quantity / foodCatalog.ServingSize;

        calories *= multiplier;
        protein *= multiplier;
        fat *= multiplier;
        carbs *= multiplier;

        var foodEntry = new FoodEntry
        {
            userId = request.UserId,
            FoodCatalogId = request.FoodCatalogId,

            FoodName = foodCatalog.FoodName,

            Quantity = request.Quantity,
            Unit = request.Unit,

            Calories = calories,
            Protein = protein,
            Fat = fat,
            Carbs = carbs
        };
         _context.FoodEntries.Add(foodEntry);

        await _context.SaveChangesAsync();

        return new AddFoodResult
        {
            Status = AddFoodStatus.Success,
            
            Food = new FoodEntryResponse
            {
            Id = foodEntry.Id,
            FoodName = foodEntry.FoodName,
            Calories = foodEntry.Calories,
            Protein = foodEntry.Protein,
            Fat = foodEntry.Fat,
            Carbs = foodEntry.Carbs,
            ConsumedAt = foodEntry.ConsumedAt
            }
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