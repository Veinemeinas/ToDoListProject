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
        private readonly ToDoListDbContext _context;
        public ToDoListRepository(ToDoListDbContext context)
        {
            _context = context;
        }

        public async Task<List<ToDo>> GetTodoListAsync(int userId)
        {
            //return await _context.TodoList.Where(tdl => tdl.UserId == userId).ToListAsync();
            return await _context.TodoList.FromSqlRaw<ToDo>("SELECT * FROM dbo.ToDoList").ToListAsync();
        }

        public async Task<ToDo> GetToDoAsync(int userId, int toDoId)
        {
            return await _context.TodoList.FirstOrDefaultAsync(tdl => tdl.UserId == userId && tdl.Id == toDoId);

        }

        public async Task<List<ToDo>> GetAllTodoList()
        {
            return await _context.TodoList.ToListAsync();
        }

        public async Task<ToDo> AddToDo(ToDo toDo)
        {
            var addedToDo = await _context.TodoList.AddAsync(toDo);
            await _context.SaveChangesAsync();
            return addedToDo.Entity;
        }

        public async Task<ToDo> UpDateToDo(ToDo todo)
        {
            var updatedToDo = _context.Update(todo);
            await _context.SaveChangesAsync();
            return updatedToDo.Entity;
        }

        public async Task<ToDo> RemoveToDo(int userId, int toDoId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                var toDo = await _context.TodoList.FirstOrDefaultAsync(tdl => tdl.Id == toDoId);
                if (toDo != null)
                {
                    _context.Remove(toDo);
                    await _context.SaveChangesAsync();
                    return toDo;
                }
            }
            return null;
        }

        public async Task<ToDo> RemoveAnyToDo(int toDoId)
        {
            var toDo = await _context.TodoList.FirstOrDefaultAsync(tdl => tdl.Id == toDoId);
            if (toDo == null)
            {
                return null;
            }
            _context.Remove(toDo);
            await _context.SaveChangesAsync();
            return toDo;
        }
    }
}
