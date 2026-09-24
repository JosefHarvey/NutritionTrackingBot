using NutritionTrackingBot.Models;

namespace NutritionTrackingBot.Services;

public interface ICalorieTrackerService
{
    Task<FoodEntryResponse?>AddFoodAsync(FoodEntry food, int userId);
    Task<DailyNutritionSummary?> GetDailySummaryAsync(int userId);
    Task<bool> ResetDailyTrackingAsync(int userId);
    Task<List<FoodEntryResponse>?>GetHistoryAsync(int userId,DateTime date);
    Task<FoodEntryResponse?> GetFoodByIdAsync(int id);
}