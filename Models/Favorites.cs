using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcFilm.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } 

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; } 

        [Required]
        public int MovieId { get; set; }

        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }
    }
}
