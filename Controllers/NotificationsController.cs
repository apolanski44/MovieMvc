using Microsoft.AspNetCore.Mvc;
using MvcFilm.Entities;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication;
using MvcFilm.Models;

[Authorize]
public class NotificationsController : Controller
{
    private readonly AppDbContext _context;

    public NotificationsController(AppDbContext context)
    {
        _context = context;
    }


    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      
        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

    
        HttpContext.Session.SetString("Notifications", JsonConvert.SerializeObject(notifications));

        return View(notifications);
    }

   

 
    [HttpPost]
    public IActionResult ClearNotifications()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

 
        var notifications = _context.Notifications.Where(n => n.UserId == userId);
        _context.Notifications.RemoveRange(notifications);
        _context.SaveChanges();

        HttpContext.Session.Remove("Notifications");

        return RedirectToAction("Index");
    }
}
