using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcFilm.Entities;
using MvcFilm.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcFilm.Controllers
{
    public class MovieController : Controller
    {
        private readonly AppDbContext _context;

        public MovieController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? genre)
        {
         
            var genres = await _context.Movies
                .Select(m => m.Genre)
                .Distinct()
                .ToListAsync();

            ViewBag.Genres = genres;

            List<Movie> movies;

            if (string.IsNullOrEmpty(genre) || genre == "All")
            {
             
                movies = await _context.Movies
                    .Select(m => new Movie
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Genre = m.Genre,
                        Description = m.Description,
                        ReleaseYear = m.ReleaseYear,
                        ImagePath = m.ImagePath,
                        AverageRating = _context.Ratings
                            .Where(r => r.MovieId == m.Id)
                            .Average(r => (double?)r.Score) ?? 0
                    })
                    .ToListAsync();
            }
            else
            {
                movies = await _context.Movies
                    .FromSqlRaw("EXEC GetMoviesByGenre @p0", genre)
                    .ToListAsync();
            }

  
            ViewBag.SelectedGenre = genre;

            return View(movies);
        }




        [Authorize]
        public async Task<IActionResult> AddToFavorites(int movieId)
        {
         
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

      
            var existingFavorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.MovieId == movieId && f.UserId == userId);

            if (existingFavorite == null)
            {
                var favorite = new Favorite
                {
                    MovieId = movieId,
                    UserId = userId
                };

                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

    
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Reviews(int movieId)
        {
         
            var movie = await _context.Movies
                .Include(m => m.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }
        public async Task<IActionResult> TopRated()
        {
            var topMovies = await _context.Movies
                .FromSqlRaw($"SELECT * FROM dbo.GetTopRatedMovies()")
                .ToListAsync();

            return View(topMovies);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RateMovie(int movieId, int score)
        {
            if (score < 1 || score > 5)
            {
                return BadRequest("Rating must be between 1 and 5.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);

            if (existingRating != null)
            {
          
                existingRating.Score = score;
                _context.Ratings.Update(existingRating);
            }
            else
            {
            
                var rating = new Rating
                {
                    MovieId = movieId,
                    UserId = userId,
                    Score = score
                };
                _context.Ratings.Add(rating);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
