using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using ToDoListProject.Dto;
using ToDoListProject.Services;
using static System.Net.Mime.MediaTypeNames;

namespace ToDoListProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult SendEmail(EmailDto emailDto)
        {
            _emailService.SendEmail(emailDto);
            return Ok();
        }
    }
}
