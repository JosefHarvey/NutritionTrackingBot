using Microsoft.AspNetCore.Mvc;
using NutritionTrackingBot.Models;
using NutritionTrackingBot.Services;

namespace NutritionTrackingBot.Controllers;

[ApiController]
[Route("api/food-catalog")]
public class FoodCatalogController : ControllerBase
{
    private readonly IFoodCatalogService _foodCatalogService;

    public FoodCatalogController(IFoodCatalogService foodCatalogService)
    {
        _foodCatalogService = foodCatalogService;
    }   

    [HttpPost]
    public async Task<IActionResult>AddFoodCatalog(AddFoodCatalogRequest request)
    {
        var foodCatalog = await _foodCatalogService.AddFoodCatalogAsync(request);
        
        return CreatedAtAction(nameof(GetFoodCatalogById), new { id = foodCatalog.Id }, foodCatalog);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFoodCatalogById(int id)
    {
        var foodCatalog =
        await _foodCatalogService.GetFoodCatalogByIdAsync(id);

        if (foodCatalog == null)
        {
            return NotFound("Food catalog not found.");
        }

        return Ok(foodCatalog);
    }
}