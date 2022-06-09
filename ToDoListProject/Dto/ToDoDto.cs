using System.ComponentModel.DataAnnotations;

namespace ToDoListProject.Dto
{
    public class ToDoDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
