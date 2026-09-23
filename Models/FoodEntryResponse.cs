namespace NutritionTrackingBot.Models;

public class FoodEntryResponse
{
    public int Id { get; set; }
    public string FoodName { get; set; } = string.Empty;
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Fat { get; set; }
    public double Carbs { get; set; }
    public DateTime ConsumedAt { get; set; }
}