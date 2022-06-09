using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDoListProject.Context;
using ToDoListProject.Model;
using ToDoListProject.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoListProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemindPasswordController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly ToDoListDbContext _dbManagementContext;

        public RemindPasswordController(IConfiguration configuration, ToDoListDbContext dbManagementContext)
        {
            _configuration = configuration;
            _dbManagementContext = dbManagementContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            var token = await GetToken(user);



            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;

            var jti = tokenS.Claims.First(claim => claim.Type == "Email").Value;

            return Ok("Email send.");
        }

        private async Task<string> GetToken(User user)
        {
            var userFind = await _dbManagementContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            if (userFind != null)
            {
                var claims = new[] {
                        new Claim("Email", user.Email),
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    null,
                    null,
                    claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: signIn);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                return null;
            }
        }
    }
}
