using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcFilm.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(50, ErrorMessage = "Max 20 characters allowed")]
        [DisplayName("Username or Email")]
        public String UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Max 20 or min 5 characters allowed")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

    }
}
