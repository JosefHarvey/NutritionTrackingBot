namespace NutritionTrackingBot.Models;

public class AddFoodResult
{
    public FoodEntryResponse? Food { get; set; }

    public AddFoodStatus Status { get; set; }
}

public enum AddFoodStatus
{
    Success,
    UserNotFound,
    FoodCatalogNotFound,
    InvalidUnit
}