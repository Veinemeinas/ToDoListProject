using System.ComponentModel.DataAnnotations;

namespace ToDoListProject.Model
{
    public class ToDo
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public bool Status { get; set; }
        public int UserId { get; set; }
    }
}
