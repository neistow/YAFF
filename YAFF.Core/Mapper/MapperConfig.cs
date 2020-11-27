using System.Linq;
using AutoMapper;
using YAFF.Core.DTO;
using YAFF.Core.Entities;

namespace YAFF.Core.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, UserInfo>();

            CreateMap<Tag, TagDto>()
                .ForMember(t => t.Id, o => o.MapFrom(t => t.TagId));
            CreateMap<Post, PostListItemDto>()
                .ForMember(p => p.Tags, o => o.MapFrom(p => p.Tags.Select(t => t.Name)));

            CreateMap<Post, PostDto>()
                .ForMember(p => p.Tags, o => o.MapFrom(p => p.Tags.Select(t => t.Name)))
                .ForMember(p => p.PostLikes, o => o.MapFrom(p => p.PostLikes.Select(l => l.UserId)));

            CreateMap<PostComment, PostCommentDto>();
        }
    }
}