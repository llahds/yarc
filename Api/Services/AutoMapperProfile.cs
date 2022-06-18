using Api.Data.Entities;
using Api.Models;
using AutoMapper;

namespace Api.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<EditForumModel, Forum>().ReverseMap();
            CreateMap<EditForumPostModel, Post>().ReverseMap();
            CreateMap<ForumModel, Forum>().ReverseMap();
            CreateMap<ForumPostModel, Post>().ReverseMap();
        }
    }
}
