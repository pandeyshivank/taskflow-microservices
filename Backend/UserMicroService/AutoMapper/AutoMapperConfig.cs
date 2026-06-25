using AutoMapper;
using UserMicroService.ModelDTOs;
using UserMicroService.Entities;

namespace UserMicroService.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() {
            CreateMap<UserRequestDTO, User>()
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.UtcNow))
                .ForMember(x => x.UpdatedAt, y => y.MapFrom(z => DateTime.UtcNow));
            CreateMap<User, UserResponseDTO>();
        }
    }
}
