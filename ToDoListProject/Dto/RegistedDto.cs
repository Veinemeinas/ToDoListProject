using System.ComponentModel.DataAnnotations;

namespace ToDoListProject.Dto
{
    public class RegistedDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(8)]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string RepeatePassword { get; set; }
    }
}
