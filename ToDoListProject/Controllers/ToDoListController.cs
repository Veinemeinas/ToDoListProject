using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using ToDoListProject.Model;
using ToDoListProject.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoListProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        private readonly ToDoListRepository _toDoListRepository;
        public ToDoListController(ToDoListRepository toDoListRepository)
        {
            _toDoListRepository = toDoListRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userIdString = User.Claims.FirstOrDefault(c => c.Type.ToString().Equals("UserId")).Value;
            var userId = Int32.Parse(userIdString);

            List<ToDo> toDoList = new List<ToDo>();
            if (User.IsInRole("Admin"))
            {
                toDoList = await _toDoListRepository.GetTodoList();
            }
            else
            {
                toDoList = await _toDoListRepository.GetTodoList(userId);
            }

            if (toDoList == null)
            {
                return NotFound();
            }
            return Ok(toDoList);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ToDo toDo)
        {

            await _toDoListRepository.AddToDoToList(toDo);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ToDo toDo)
        {
            await _toDoListRepository.UpDateToDoList(toDo);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdString = User.Claims.FirstOrDefault(c => c.Type.ToString().Equals("UserId")).Value;
            var userId = Int32.Parse(userIdString);

            await _toDoListRepository.RemoveToDo(userId, id);
            return Ok();
        }
    }
}
