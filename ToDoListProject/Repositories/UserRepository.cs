using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ToDoListProject.Context;
using ToDoListProject.Model;

namespace ToDoListProject.Repositories
{
    public class UserRepository
    {
        private readonly DbManagementContext _dbManagementContext;

        public UserRepository(DbManagementContext dbManagementContext)
        {
            _dbManagementContext = dbManagementContext;
        }

        public async Task<User> GetUser(string email, string password)
        {
            var user = await _dbManagementContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                return user;
            }
            return null;
        }
    }
}
