using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcFilm.Entities;
using MvcFilm.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcFilm.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly AppDbContext _context;

        public FavoritesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           
            var favorites = await _context.Favorites
                .Include(f => f.Movie)
                .ThenInclude(m => m.Ratings) 
                .Where(f => f.UserId == userId)
                .ToListAsync();

            foreach (var favorite in favorites)
            {
                if (favorite.Movie.Ratings != null && favorite.Movie.Ratings.Any())
                {
                    favorite.Movie.AverageRating = favorite.Movie.Ratings.Average(r => r.Score);
                }
                else
                {
                    favorite.Movie.AverageRating = 0; 
                }
            }

            return View(favorites);
        }


        [HttpPost]
        public async Task<IActionResult> Add(int movieId)
        {
       
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

    
            var existingFavorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.MovieId == movieId);

            if (existingFavorite == null)
            {
                var favorite = new Favorite
                {
                    UserId = userId,
                    MovieId = movieId
                };

                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Movie");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int favoriteId)
        {
            var favorite = await _context.Favorites.FindAsync(favoriteId);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
