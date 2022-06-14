using ToDoListProject.Dto;

namespace ToDoListProject.Services
{
    public interface IEmailService
    {
        void SendEmail(string to, string message);
    }
}
