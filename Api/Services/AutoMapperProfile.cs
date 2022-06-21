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
            CreateMap<ForumPostListItemModel, Post>().ReverseMap();
            CreateMap<RegisterModel, User>().ReverseMap();
            CreateMap<UserSettingsModel, User>().ReverseMap();
            CreateMap<ReportReasonModel, ReportReason>().ReverseMap();
            CreateMap<UserInfoModel, User>().ReverseMap();
            CreateMap<ForumPostSettingsModel, ForumPostSettings>().ReverseMap();
            CreateMap<ForumPostGuideLinesModel, ForumPostSettings>().ReverseMap();
        }
    }
}
