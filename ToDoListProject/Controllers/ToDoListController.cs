using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using ToDoListProject.Context;
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
            var userId = GetUserId();
            var toDoList = await _toDoListRepository.GetTodoList(userId);
            return Ok(toDoList);
        }

        [HttpGet, Route("{toDoId}")]
        public async Task<IActionResult> GetToDo(int toDoId)
        {
            var userId = GetUserId();
            var toDo = await _toDoListRepository.GetToDo(userId, toDoId);
            return Ok(toDo);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ToDo toDo)
        {
            toDo.Id = 0;
            toDo.UserId = GetUserId();
            var addedToDo = await _toDoListRepository.AddToDo(toDo);
            return Created("", addedToDo);
        }

        [HttpPut, Route("{toDoId}")]
        public async Task<IActionResult> UpdateToDo(int toDoId, [FromBody] ToDo toDo)
        {
            if (toDo == null)
            {
                return BadRequest();
            }

            var userId = GetUserId();
            ToDo newToDo = new ToDo() { Id = toDoId, Title = toDo.Title, Status = toDo.Status, UserId = userId };

            var toDoAdded = await _toDoListRepository.UpDateToDo(newToDo);
            return Ok(toDoAdded);
        }

        [HttpDelete, Route("{toDoId}")]
        public async Task<IActionResult> RemoveToDo(int toDoId)
        {
            var userId = GetUserId();
            var removedToDo = await _toDoListRepository.RemoveToDo(userId, toDoId);
            if (removedToDo == null)
            {
                return NotFound();
            }

            return Ok(removedToDo);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("admin")]
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
            var removedToDo = await _toDoListRepository.RemoveEnyToDo(toDoId);
            return Ok(removedToDo);
        }

        private int GetUserId()
        {
            return int.Parse(User.Claims.First(c => c.Type == "UserId").Value);
        }
    }
}
