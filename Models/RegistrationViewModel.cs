using System.ComponentModel.DataAnnotations;

namespace MvcFilm.Models
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed")]
        public String FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed")]
        public String LastName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please Enter Valid Email.")]
        public String Email { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(50, ErrorMessage = "Max 20 characters allowed")]

        public String UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Max 20 or min 5 characters allowed" )]
        [DataType(DataType.Password)]
        public String Password { get; set; }


        [Compare("Password", ErrorMessage = "Confirm your password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
