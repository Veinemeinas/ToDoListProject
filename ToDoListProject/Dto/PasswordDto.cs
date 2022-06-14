using System.ComponentModel.DataAnnotations;

namespace ToDoListProject.Dto
{
    public class PasswordDto
    {
        [Required]
        public string Token { get; set; }
        [Required, MinLength(8)]
        public string NewPassword { get; set; }
        [Required, Compare("NewPassword", ErrorMessage = "Passwords didn't mach!")]
        public string NewPasswordRepeat { get; set; }
    }
}
