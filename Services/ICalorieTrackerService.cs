using NutritionTrackingBot.Models;

namespace NutritionTrackingBot.Services;

public interface ICalorieTrackerService
{
    Task AddFoodAsync(FoodEntry food);
    Task<DailyNutritionSummary> GetDailySummaryAsync(int userId);
    Task ResetDailyTrackingAsync();
    Task<List<FoodEntryResponse>>GetHistoryAsync(int userId,DateTime date);
}