using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcFilm.Models
{
    public class Comment
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

        [Required(ErrorMessage = "Comment cannot be empty")]
        [MaxLength(500)]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
