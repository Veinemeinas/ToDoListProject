using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using ToDoListProject.Context;
using ToDoListProject.Model;
using ToDoListProject.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoListProject.Controllers
{
    [Authorize]
    [Route("api")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        private readonly DbManagementContext _dbManagementContext;
        private readonly ToDoListRepository _toDoListRepository;
        public ToDoListController(ToDoListRepository toDoListRepository, DbManagementContext dbManagementContext)
        {
            _toDoListRepository = toDoListRepository;
            _dbManagementContext = dbManagementContext;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetAllToDoList")]
        public async Task<IActionResult> GetAll()
        {
            var toDoList = await _dbManagementContext.TodoList.ToListAsync();
            if (toDoList == null)
            {
                return BadRequest(new { error = "ToDoList not found." });
            }
            return Ok(toDoList);
        }

        [HttpGet]
        [Route("GetMyToDoList")]
        public async Task<IActionResult> GefMy()
        {
            var userId = GetUserId();
            var toDoList = await _dbManagementContext.TodoList.Where(t => t.UserId == userId).ToListAsync();
            if (toDoList == null)
            {
                return BadRequest(new { error = "ToDoList not found." });
            }
            return Ok(toDoList);
        }

        [HttpPost]
        [Route("AddToDo")]
        public async Task<IActionResult> Post([FromBody] ToDo toDo)
        {
            toDo.UserId = GetUserId();
            await _dbManagementContext.TodoList.AddAsync(toDo);
            await _dbManagementContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Route("UpdateToDo")]
        public async Task<IActionResult> Put([FromBody] ToDo toDo)
        {
            var userId = GetUserId();
            var toDoFind = await _dbManagementContext.TodoList.FirstOrDefaultAsync(t => t.UserId == toDo.UserId);
            if (toDoFind == null)
            {
                return BadRequest(new { error = "ToDo not found." });
            }
            if (toDo.UserId != userId)
            {
                return BadRequest(new { error = "UserId can't be changed." });
            }

            _dbManagementContext.TodoList.Update(toDo);
            await _dbManagementContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("RemoveToDo")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();
            var toDoFind = await _dbManagementContext.TodoList.FirstOrDefaultAsync(t => t.Id == id);
            if (toDoFind == null)
            {
                return BadRequest(new { error = "ToDo not found." });
            }
            if (toDoFind.UserId != userId)
            {
                return BadRequest(new { error = "ToDo can't be removed" });
            }

            _dbManagementContext.TodoList.Remove(toDoFind);
            await _dbManagementContext.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("RemoveOthersToDo")]
        public async Task<IActionResult> DeleteOtherTodo(int id)
        {
            var userId = GetUserId();
            var toDoFind = await _dbManagementContext.TodoList.FirstOrDefaultAsync(t => t.Id == id);
            if (toDoFind == null)
            {
                return BadRequest(new { error = "ToDo not found." });
            }

            _dbManagementContext.TodoList.Remove(toDoFind);
            await _dbManagementContext.SaveChangesAsync();
            return Ok();
        }

        private int GetUserId()
        {
            return int.Parse(User.Claims.First(c => c.Type == "UserId").Value);
        }
    }
}
