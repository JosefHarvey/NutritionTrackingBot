using NutritionTrackingBot.Models;

namespace NutritionTrackingBot.Services;

public class CalorieTrackerService : ICalorieTrackerService
{
    private readonly List<FoodEntry> _foods = new();

    public void AddFood(FoodEntry food)
    {
        _foods.Add(food);
    }

    public DailyNutritionSummary GetDailySummary()
    {
        return new DailyNutritionSummary
        {
            Calories = _foods.Sum(f => f.Calories),
            Protein = _foods.Sum(f => f.Protein),
            Fat = _foods.Sum(f => f.Fat),
            Carbs = _foods.Sum(f => f.Carbs)
        };
    }

    public void ResetDailyTracking()
    {
        _foods.Clear();
    }
}