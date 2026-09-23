using Microsoft.AspNetCore.Mvc;
using NutritionTrackingBot.Data;
using NutritionTrackingBot.Models;

namespace NutritionTrackingBot.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(User user)
    {
        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return Ok(user);
    }
}