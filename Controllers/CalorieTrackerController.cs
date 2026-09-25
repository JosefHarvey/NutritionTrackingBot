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
        var result = await _trackerService.AddFoodAsync(request);

        switch (result.Status)
        {
            case AddFoodStatus.UserNotFound:
                return NotFound("User not found.");

            case AddFoodStatus.FoodCatalogNotFound:
                return NotFound("Food catalog not found.");

            case AddFoodStatus.InvalidUnit:
                return BadRequest(
                    "The selected unit does not match the food catalog serving unit.");

            case AddFoodStatus.Success:
                return CreatedAtAction(
                    nameof(GetFoodById),
                    new { id = result.Food!.Id },
                    result.Food);

            default:
                return BadRequest("Failed to add food.");
        }
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