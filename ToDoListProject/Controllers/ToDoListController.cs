using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToDoListProject.Context;
using ToDoListProject.Dto;
using ToDoListProject.Model;
using ToDoListProject.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoListProject.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/todolist")]
    public class ToDoListController : ControllerBase
    {
        private readonly ToDoListDbContext _context;
        private readonly ToDoListRepository _toDoListRepository;
        public ToDoListController(ToDoListDbContext context, ToDoListRepository repository)
        {
            _context = context;
            _toDoListRepository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyList()
        {
            var userId = GetCurrentUserId();
            var toDoList = await _toDoListRepository.GetTodoList(userId);
            return Ok(toDoList);
        }

        [HttpGet, Route("{toDoId}")]
        public async Task<IActionResult> GetToDo(int toDoId)
        {
            var userId = GetCurrentUserId();
            var toDo = await _toDoListRepository.GetToDo(userId, toDoId);
            return Ok(toDo);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ToDoDto toDoDto)
        {
            ToDo toDo = new ToDo();
            toDo.Id = 0;
            toDo.Title = toDoDto.Title;
            toDo.Status = toDoDto.Status;
            toDo.UserId = GetCurrentUserId();
            var addedToDo = await _toDoListRepository.AddToDo(toDo);
            return Created("", addedToDo);
        }

        [HttpPut, Route("{toDoId}")]
        public async Task<IActionResult> UpdateToDo(int toDoId, [FromBody] ToDoDto toDoDto)
        {
            var userId = GetCurrentUserId();
            if (toDoId == userId)
            {
                ToDo newToDo = new ToDo() { Id = toDoId, Title = toDoDto.Title, Status = toDoDto.Status, UserId = userId };
                var toDoAdded = await _toDoListRepository.UpDateToDo(newToDo);
                return Ok(toDoAdded);
            }
            return BadRequest();
        }

        [HttpDelete, Route("{toDoId}")]
        public async Task<IActionResult> RemoveToDo(int toDoId)
        {
            var userId = GetCurrentUserId();
            var removedToDo = await _toDoListRepository.RemoveToDo(userId, toDoId);
            if (removedToDo == null)
            {
                return NotFound();
            }

            return Ok(removedToDo);
        }

        [HttpGet, Route("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var toDoList = await _toDoListRepository.GetAllTodoList();
            if (toDoList == null)
            {
                return NotFound();
            }
            return Ok(toDoList);
        }

        [HttpDelete, Route("admin/{toDoId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveAnydo(int toDoId)
        {
            var removedToDo = await _toDoListRepository.RemoveAnyToDo(toDoId);
            return Ok(removedToDo);
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        }
    }
}
