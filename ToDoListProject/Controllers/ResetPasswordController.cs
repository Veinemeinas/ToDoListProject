using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ToDoListProject.Dto;
using ToDoListProject.Repositories;

namespace ToDoListProject.Controllers
{
    [Route("api")]
    [ApiController]
    public class ResetPasswordController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        public ResetPasswordController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(EmailDto emailDto)
        {
            var user = await _userRepository.GetUser(emailDto.Email);
            if (user == null)
            {
                return BadRequest(error: "User not Found!");
            }
            await _userRepository.SendResetPasswordEmail(user);

            return Ok("Email with password reset link send.");
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(PasswordDto passwordDto)
        {
            if (!await _userRepository.ValidateToken(passwordDto))
            {
                return BadRequest(error: "User not Found!");
            }

            return Ok("Password changed.");
        }
    }
}
