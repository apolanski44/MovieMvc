using System.Collections.Generic;

namespace MvcFilm.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public int ReleaseYear { get; set; }
        public string ImagePath { get; set; }
        public double AverageRating { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Rating> Ratings { get; set; } = new List<Rating>();




    }
}
