namespace NutritionTrackingBot.Models;

public class UserFoodCatalog
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public int FoodCatalogId { get; set; }

    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Fat { get; set; }
    public double Carbs { get; set; }

    public string Source { get; set; } = string.Empty;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
    public FoodCatalog? FoodCatalog { get; set; }
}