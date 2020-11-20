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
        }
    }
}