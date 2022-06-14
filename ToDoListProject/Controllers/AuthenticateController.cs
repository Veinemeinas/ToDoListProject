using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using ToDoListProject.Dto;
using ToDoListProject.Repositories;

namespace ToDoListProject.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly UserRepository _userRepository;

        public AuthenticateController(IConfiguration configuration, UserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        [HttpPost, Route("Login")]
        public async Task<IActionResult> Login([FromBody] AuthDto auth)
        {
            var token = await _userRepository.GenerateToken(auth.Email, auth.Password);

            if (token != null)
            {
                return Ok(token);
            }

            return BadRequest("Invalid credentials");
        }

        [HttpPost, Route("Register")]
        public async Task<IActionResult> CreateUser([FromBody] AuthDto authDto)
        {
            var exist = await _userRepository.CreateUser(authDto);
            if (!exist)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
