using Microsoft.EntityFrameworkCore;

namespace ToDoListProject.Context
{
    public class DbManagementContext : DbContext
    {
        public DbManagementContext(DbContextOptions<DbManagementContext> options) : base(options)
        {

        }
    }
}
