using System.ComponentModel.DataAnnotations;

namespace NutritionTrackingBot.Models;

public class AddFoodRequest
{
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    [Range(1, int.MaxValue)]
    public int FoodCatalogId { get; set; }

    [Range(0.01, double.MaxValue)]
    public double Quantity { get; set; }

    public FoodUnit Unit { get; set; }
}