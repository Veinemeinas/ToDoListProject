using System.Collections.Generic;

namespace ToDoListProject.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public string PasswordHash { get; set; }
        public List<ToDo> ToDoList { get; set; }
        public Role Role { get; set; }
        public static object Claims { get; internal set; }
    }
}
