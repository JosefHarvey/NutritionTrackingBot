using NutritionTrackingBot.Models;

namespace NutritionTrackingBot.Services;

public interface ICalorieTrackerService
{
    void AddFood(FoodEntry food);
    DailyNutritionSummary GetDailySummary();
    void ResetDailyTracking();
}