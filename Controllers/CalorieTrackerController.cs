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
    public IActionResult GetToday()
    {
        var summary = _trackerService.GetDailySummary();

        return Ok(summary);
    }

    [HttpPost("food")]
    public IActionResult AddFood(FoodEntry food)
    {
        _trackerService.AddFood(food);

        return Ok("Food added successfully.");
    }
}