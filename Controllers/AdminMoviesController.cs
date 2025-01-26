using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcFilm.Entities;
using MvcFilm.Models;
using System.Linq;

namespace MvcFilm.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminMoviesController : Controller
    {
        private readonly AppDbContext _context;

        public AdminMoviesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var movies = _context.Movies.ToList();
            return View(movies);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Movies.Add(movie);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        public IActionResult Edit(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Movies.Update(movie);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.Id == id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
