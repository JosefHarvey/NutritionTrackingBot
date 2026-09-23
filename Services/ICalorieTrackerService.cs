using NutritionTrackingBot.Models;

namespace NutritionTrackingBot.Services;

public interface ICalorieTrackerService
{
    Task AddFoodAsync(FoodEntry food);
    Task<DailyNutritionSummary> GetDailySummaryAsync();
    Task ResetDailyTrackingAsync();
}