using Microsoft.AspNetCore.Mvc;
using NutritionTrackingBot.Models;
using NutritionTrackingBot.Services;

namespace NutritionTrackingBot.Controllers;

[ApiController]
[Route("api/calorie-tracker")]
public class CalorieTrackerController : ControllerBase
{
    private readonly ICalorieTrackerService _trackerService;

    public CalorieTrackerController(ICalorieTrackerService trackerService)
    {
        _trackerService = trackerService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Calorie Tracker API is running!");
    }

    [HttpGet("today")]
    public async Task<IActionResult> GetToday([FromQuery] int userId)
    {
        var summary = await _trackerService.GetDailySummaryAsync(userId);

        if (summary == null)
        {
            return NotFound("User not found.");
        }

        return Ok(summary);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory([FromQuery] int userId, [FromQuery] DateTime date)
    {
        var history = await _trackerService.GetHistoryAsync(userId, date);

        if (history == null)
        {
            return NotFound("User not found.");
        }
        
        return Ok(history);
    }

    [HttpGet("food/{id}")]
    public async Task<IActionResult> GetFoodById(int id)
    {
        var food = await _trackerService.GetFoodByIdAsync(id);

        if (food == null)
        {
            return NotFound("Food entry not found.");
        }

        return Ok(food);
    }

    [HttpPost("food")]
    public async Task<IActionResult> AddFood(AddFoodRequest request)
    {
        var food = new FoodEntry
        {
            FoodName = request.FoodName,
            Calories = request.Calories,
            Protein = request.Protein,
            Fat = request.Fat,
            Carbs = request.Carbs
        };

        var result = await _trackerService.AddFoodAsync(food,request.UserId);

        if (result == null)
        {
            return NotFound("User not found.");
        }

        return CreatedAtAction(nameof(GetFoodById), new { id = result.Id }, result);
    }

    [HttpPost("reset")]
    public async Task<IActionResult> Reset([FromQuery] int userId)
    {
        var success = await _trackerService.ResetDailyTrackingAsync(userId);

        if (!success)
        {
            return NotFound("User not found.");
        }       
        return Ok(new
        {
            message = "Daily tracking reset successfully."
        });
    }
    
}