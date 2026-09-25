using System.ComponentModel.DataAnnotations;

namespace NutritionTrackingBot.Models;

public class AddFoodCatalogRequest
{
    [Required]
    public string FoodName { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public double ServingSize { get; set; }

    public FoodUnit ServingUnit { get; set; }

    [Range(0, double.MaxValue)]
    public double Calories { get; set; }

    [Range(0, double.MaxValue)]
    public double Protein { get; set; }

    [Range(0, double.MaxValue)]
    public double Fat { get; set; }

    [Range(0, double.MaxValue)]
    public double Carbs { get; set; }

    [Required]
    public string Source { get; set; } = string.Empty;
}