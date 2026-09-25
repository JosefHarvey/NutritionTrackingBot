using NutritionTrackingBot.Models;

namespace NutritionTrackingBot.Services;

public interface IFoodCatalogService
{
    Task<FoodCatalog> AddFoodCatalogAsync(AddFoodCatalogRequest request);
    Task<FoodCatalog?> GetFoodCatalogByIdAsync(int id);
}