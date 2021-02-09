using System.Linq;
using AutoMapper;
using YAFF.Core.DTO;
using YAFF.Core.Entities;
using YAFF.Core.Entities.Identity;

namespace YAFF.Core.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, UserDto>()
                .ForMember(u => u.Avatar,
                    o => o.MapFrom(u => $"files/pictures/{u.Profile.Avatar.FileName}"));
            CreateMap<User, AuthorDto>()
                .ForMember(a => a.Avatar,
                    o => o.MapFrom(u => $"files/pictures/{u.Profile.Avatar.FileName}"));
            CreateMap<UserProfile, UserProfileDto>()
                .ForMember(u => u.UserName,
                    o => o.MapFrom(u => u.User.UserName))
                .ForMember(u => u.Avatar,
                    o => o.MapFrom(u => $"files/pictures/{u.Avatar.FileName}"));


            CreateMap<Tag, TagDto>()
                .ForMember(t => t.Id,
                    o => o.MapFrom(t => t.Id));
            CreateMap<Post, PostListItemDto>()
                .ForMember(p => p.Author,
                    o => o.MapFrom(p => p.Author))
                .ForMember(p => p.Summary,
                    o => o.MapFrom(p => p.Preview.Body))
                .ForMember(p => p.PreviewImage,
                    o => o.MapFrom(p => $"files/pictures/{p.Preview.Image.FileName}"))
                .ForMember(p => p.Tags,
                    o => o.MapFrom(p => p.PostTags.Select(pt => pt.Tag.Name)));

            CreateMap<PostPreview, PostPreviewDto>()
                .ForMember(p => p.Image,
                    o => o.MapFrom(p => $"files/pictures/{p.Image.FileName}"));

            CreateMap<Post, PostDto>()
                .ForMember(p => p.Author,
                    o => o.MapFrom(p => p.Author))
                .ForMember(p => p.Tags,
                    o => o.MapFrom(p => p.PostTags.Select(t => t.Tag.Name)))
                .ForMember(p => p.PostLikes,
                    o => o.MapFrom(p => p.PostLikes.Select(l => l.UserId)))
                .ForMember(p => p.Preview,
                    o => o.MapFrom(p => p.Preview));

            CreateMap<Comment, CommentDto>();
        }
    }
}