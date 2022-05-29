using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListProject.Context;
using ToDoListProject.Model;

namespace ToDoListProject.Repositories
{
    public class ToDoListRepository
    {
        private readonly DbManagementContext _dbManagementContext;
        public ToDoListRepository(DbManagementContext dbManagementContext)
        {
            _dbManagementContext = dbManagementContext;
        }

        public async Task<List<ToDo>> GetTodoList(int userId)
        {
            var user = await _dbManagementContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return null;
            }
            return await _dbManagementContext.TodoList.Where(tdl => tdl.UserId == user.Id).ToListAsync();
        }

        public async Task<List<ToDo>> GetTodoList()
        {
            var user = await _dbManagementContext.TodoList.ToListAsync();
            if (user == null)
            {
                return null;
            }
            return user;
        }

        public async Task AddToDoToList(ToDo toDo)
        {
            await _dbManagementContext.TodoList.AddAsync(toDo);
            await _dbManagementContext.SaveChangesAsync();
        }

        public async Task UpDateToDoList(ToDo todo)
        {
            _dbManagementContext.Update(todo);
            await _dbManagementContext.SaveChangesAsync();
        }

        public async Task RemoveToDo(int userId, int toDoId)
        {
            var user = await _dbManagementContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                var toDo = await _dbManagementContext.TodoList.FirstOrDefaultAsync(r => r.Id == toDoId);
                if (toDo != null)
                {
                    _dbManagementContext.Remove(toDo);
                    await _dbManagementContext.SaveChangesAsync();
                }
            }
        }
    }
}
