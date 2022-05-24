using Microsoft.EntityFrameworkCore;
using ToDoListProject.Model;

namespace ToDoListProject.Context
{
    public class DbManagementContext : DbContext
    {
        public DbManagementContext(DbContextOptions<DbManagementContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<ToDo> TodoList { get; set; }
    }
}
