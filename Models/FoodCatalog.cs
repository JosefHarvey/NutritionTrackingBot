namespace NutritionTrackingBot.Models;

public class FoodCatalog
{
    public int Id { get; set; }
    public string FoodName { get; set; } = string.Empty;
    public double ServingSize { get; set; }
    public FoodUnit ServingUnit { get; set; }
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Fat { get; set; }
    public double Carbs { get; set; }
    public string Source { get; set; } = string.Empty;
}