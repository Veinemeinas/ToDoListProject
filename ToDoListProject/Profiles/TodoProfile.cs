using AutoMapper;
using ToDoListProject.Dto;
using ToDoListProject.Model;

namespace ToDoListProject.Profiles
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<ToDoDto, ToDo>();
        }
    }
}
