using System.ComponentModel.DataAnnotations;

namespace ToDoListProject.Dto
{
    public class ToDoResponseDto
    {
        [Required]
        public int Id { get; set; }
        public string Title { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
