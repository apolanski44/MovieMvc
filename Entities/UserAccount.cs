using Microsoft.EntityFrameworkCore;
using MvcFilm.Models;
using System.ComponentModel.DataAnnotations;
namespace MvcFilm.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(UserName), IsUnique = true)]
    public class UserAccount
    {
        [Key] 
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed")]
        public String FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed")]
        public String LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed")]
        public String Email { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(50, ErrorMessage = "Max 20 characters allowed")]
        public String UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MaxLength(50, ErrorMessage = "Max 20 characters allowed")]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();



    }
}
