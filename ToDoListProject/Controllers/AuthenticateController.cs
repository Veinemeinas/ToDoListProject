using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

        [HttpPost, Route("Auth")]
        public async Task<IActionResult> Post([FromBody] UserDto userDto)
        {
            if (userDto != null && userDto.Email != null && userDto.Password != null)
            {
                string hash = _userRepository.EncodePassword(userDto.Password);

                var user = await _userRepository.GetUser(userDto.Email, hash);

                if (user != null)
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim("UserId", user.Id.ToString()),
                        new Claim("Email", user.Email),
                        new Claim(ClaimTypes.Role, user.Role.RoleName)
                    };

                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));
                    var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);
                    var token = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.UtcNow.AddHours(1),
                        signingCredentials: credentials);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            await _userRepository.CreateUser(userDto);
            return Ok();
        }
    }
}
