using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System;
using ToDoListProject.Dto;

namespace ToDoListProject.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(string to, string message)
        {
            string server = _config["EmailConfig:Server"];
            int port = Convert.ToInt32(_config["EmailConfig:port"]);
            string email = _config["EmailConfig:Email"];
            string password = _config["EmailConfig:Password"];

            MimeMessage mimeMessage = new MimeMessage();
            mimeMessage.From.Add(MailboxAddress.Parse(email));
            mimeMessage.To.Add(MailboxAddress.Parse(to));
            mimeMessage.Subject = "Pasword reset Link";

            mimeMessage.Body = new TextPart(TextFormat.Html) { Text = $"<a href=\"https://pvzmanopuslapioadresas.lt/slaptazodiokeitimopsl.js?token={message}\">Atnaujinti slaptažodį</a>" };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(server, port, SecureSocketOptions.StartTls);
                smtp.Authenticate(email, password);
                smtp.Send(mimeMessage);
                smtp.Disconnect(true);
            }
        }
    }
}

