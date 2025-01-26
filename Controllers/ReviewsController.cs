using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcFilm.Entities;
using MvcFilm.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcFilm.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; 

  
        public ReviewsController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager; 
        }

        public async Task<IActionResult> Index(int movieId)
        {
            var movie = await _context.Movies
                .Include(m => m.Comments)
                .ThenInclude(c => c.User)
                .Include(m => m.Ratings)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                return NotFound();
            }

            if (movie.Ratings != null && movie.Ratings.Any())
            {
                movie.AverageRating = movie.Ratings.Average(r => r.Score);
            }
            var user = await _userManager.GetUserAsync(User);

         
            ViewBag.UserId = user?.Id;

            return View(movie);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int commentId)
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(); 
            }

         
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                return NotFound(); 
            }

            if (comment.UserId != user.Id)
            {
                return Forbid(); 
            }

  
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

      
            return RedirectToAction("Index", new { movieId = comment.MovieId });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(int movieId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                ModelState.AddModelError("", "Comment cannot be empty.");
                return RedirectToAction("Index", new { movieId });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

      
            var comment = new Comment
            {
                MovieId = movieId,
                UserId = user.Id, 
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { movieId });
        }
    }
}
