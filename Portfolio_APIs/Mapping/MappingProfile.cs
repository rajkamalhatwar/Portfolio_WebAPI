using AutoMapper;
using ProjectAPI.Entity;
using ProjectAPI.ViewModel;

namespace ProjectAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDetail, VMUserDetail>();
            CreateMap<UserEntity, VMUser>();
        }
    }
}
