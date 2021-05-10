using AutoMapper;
using DTO;
using Models;

namespace Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User,UserViewModel>();
            CreateMap<CreateUser,User>();
            CreateMap<Message,MessageDTO>();
        }
    }
}