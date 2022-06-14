using System.ComponentModel.DataAnnotations;

namespace ToDoListProject.Dto
{
    public class EmailDto
    {
        [Required, EmailAddress(ErrorMessage = "Email address is not valid!")]
        public string Email { get; set; }
    }
}
