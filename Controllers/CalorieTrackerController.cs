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

        return Ok(summary);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory([FromQuery] int userId, [FromQuery] DateTime date)
    {
        var history = await _trackerService.GetHistoryAsync(userId, date);

        return Ok(history);
    }

    [HttpPost("food")]
    public async Task<IActionResult> AddFood(FoodEntry food)
    {
        await _trackerService.AddFoodAsync(food);

        return Ok("Food added successfully.");
    }
    
}