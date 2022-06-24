using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using ToDoListProject.Dto;
using ToDoListProject.Repositories;

namespace ToDoListProject.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public AuthenticateController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] AuthDto auth)
        {
            var token = await _userRepository.GenerateToken(auth);

            if (token == null)
            {
                return NotFound();
            }

            return Ok(token);
        }

        [HttpPost, Route("register")]
        public async Task<IActionResult> CreateUser([FromBody] RegistedDto registedDto)
        {
            var exist = await _userRepository.CreateUser(registedDto);
            if (exist == false)
            {
                return BadRequest(error: $"User with {registedDto.Email} username already exist.");
            }

            return Ok();
        }
    }
}
