namespace NutritionTrackingBot.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TimeZoneId { get; set; } = "UTC";
    public List<FoodEntry> FoodEntries { get; set; } = new List<FoodEntry>();
}