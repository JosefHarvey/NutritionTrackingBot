using System.ComponentModel.DataAnnotations;

namespace NutritionTrackingBot.Models;

public class AddFoodRequest
{
    [Required]
    public string FoodName {get;set;} = string.Empty;
    
    [Range(0,double.MaxValue)]
    public double Calories { get; set; }
    [Range(0,double.MaxValue)]
    public double Protein { get; set; }
    [Range(0,double.MaxValue)]
    public double Fat { get; set; }
    [Range(0,double.MaxValue)]
    public double Carbs { get; set; }

    public int UserId { get; set; }
}