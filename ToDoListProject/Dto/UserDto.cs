using System.ComponentModel.DataAnnotations;

namespace ToDoListProject.Dto
{
    public class UserDto
    {
        [Required, EmailAddress(ErrorMessage = "Email format not valid!")]
        public string Email { get; set; }
        [Required, MinLength(8, ErrorMessage = "Pasword must be at least 8 chapters long!")]
        public string Password { get; set; }
    }
}
