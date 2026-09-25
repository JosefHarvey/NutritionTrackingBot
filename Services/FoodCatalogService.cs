using NutritionTrackingBot.Data;
using NutritionTrackingBot.Models;
using Microsoft.EntityFrameworkCore;

namespace NutritionTrackingBot.Services;

public class FoodCatalogService : IFoodCatalogService
{
    private readonly AppDbContext _context;

    public FoodCatalogService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FoodCatalog> AddFoodCatalogAsync(AddFoodCatalogRequest request)
    {
        var foodCatalog = new FoodCatalog
        {
            FoodName = request.FoodName,
            ServingSize = request.ServingSize,
            ServingUnit = request.ServingUnit,
            Calories = request.Calories,
            Protein = request.Protein,
            Fat = request.Fat,
            Carbs = request.Carbs,
            Source = request.Source
        };

        _context.FoodCatalogs.Add(foodCatalog);

        await _context.SaveChangesAsync();

        return foodCatalog;
    }

    public async Task<FoodCatalog?> GetFoodCatalogByIdAsync(int id)
{
    return await _context.FoodCatalogs
        .FirstOrDefaultAsync(f => f.Id == id);
}
}