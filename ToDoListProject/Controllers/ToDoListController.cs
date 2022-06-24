using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDoListProject.Context;
using ToDoListProject.Dto;
using ToDoListProject.Model;
using ToDoListProject.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoListProject.Controllers
{
    [ApiController, Authorize, Route("api/todolist")]
    public class ToDoListController : ControllerBase
    {
        private readonly ToDoListRepository _toDoListRepository;
        private readonly IMapper _mapper;
        public ToDoListController(ToDoListDbContext context, ToDoListRepository repository, IMapper mapper)
        {
            _toDoListRepository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyList()
        {
            var userId = GetCurrentUserId();
            var toDoList = await _toDoListRepository.GetTodoListAsync(userId);

            if (toDoList == null)
            {
                return NotFound();
            }

            var toDoResponseDtos = _mapper.Map<List<ToDoResponseDto>>(toDoList);

            return Ok(toDoResponseDtos);
        }

        [HttpGet, Route("{toDoId}")]
        public async Task<IActionResult> GetToDo(int toDoId)
        {
            var userId = GetCurrentUserId();
            var toDo = await _toDoListRepository.GetToDoAsync(userId, toDoId);

            if (toDo == null)
            {
                return NotFound();
            }

            var toDoResponseDtos = _mapper.Map<ToDoResponseDto>(toDo);

            return Ok(toDoResponseDtos);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ToDoDto toDoDto)
        {
            try
            {
                var toDo = _mapper.Map<ToDo>(toDoDto);
                toDo.UserId = GetCurrentUserId();
                var addedToDo = await _toDoListRepository.AddToDo(toDo);
                return Created($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}/{addedToDo.Id}", addedToDo);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut, Route("{toDoId}")]
        public async Task<IActionResult> UpdateToDo(int toDoId, [FromBody] ToDoDto toDoDto)
        {
            var userId = GetCurrentUserId();
            var todo = await _toDoListRepository.GetToDoAsync(userId, toDoId);

            if (todo == null)
            {
                return NotFound();
            }

            var toDo = _mapper.Map<ToDo>(toDoDto);
            todo.UserId = userId;
            var toDoAdded = await _toDoListRepository.UpDateToDo(todo);
            return Ok(toDoAdded);

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

            return NoContent();
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
            return NoContent();
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
        }
    }
}
