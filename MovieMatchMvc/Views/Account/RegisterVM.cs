using System.ComponentModel.DataAnnotations;

namespace MovieMatchMvc.Views.Account
{
    public class RegisterVM
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [Display(Name = "E-Mail")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        [Compare(nameof(Password))]
        public string PasswordRepeat { get; set; }
    }
}

