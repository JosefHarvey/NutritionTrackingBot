using System.Text.Json.Serialization;

namespace NutritionTrackingBot.Models;

public class FoodEntry
{public int Id { get; set; }

    // User
    public int userId { get; set; }
    [JsonIgnore]
    public User? user { get; set; }

    // Food Catalog
    public int FoodCatalogId { get; set; }
    [JsonIgnore]
    public FoodCatalog? FoodCatalog { get; set; }

    // Snapshot of the food information at the time of consumption
    public string FoodName { get; set; } = string.Empty;
    public double Quantity { get; set; }
    public FoodUnit Unit { get; set; }

    // Nutrition snapshot
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Fat { get; set; }
    public double Carbs { get; set; }
    public DateTime ConsumedAt { get; set; } = DateTime.UtcNow;
}