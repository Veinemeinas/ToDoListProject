using ToDoListProject.Dto;

namespace ToDoListProject.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailDto eMail);
    }
}
