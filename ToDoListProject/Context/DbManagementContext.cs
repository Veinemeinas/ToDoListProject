using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoListProject.Model;

namespace ToDoListProject.Context
{
    public class ToDoListDbContext : DbContext
    {
        public ToDoListDbContext(DbContextOptions<ToDoListDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ToDo> TodoList { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
