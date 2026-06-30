using AutoMapper;
using TaskMicroService.Entities;
using TaskMicroService.ModelDTOs;

namespace TaskMicroService.AutoMapper
{
    public class TaskMappingConfig : Profile
    {
     
        public TaskMappingConfig() {

            CreateMap<TaskRequestDTO, TaskItem>()
                .ForMember(x => x.CreatedAt, y => y.MapFrom(z => DateTime.UtcNow))
               .ForMember(x => x.UpdatedAt, y => y.MapFrom(z => DateTime.UtcNow));
            ;
            CreateMap<TaskItem,TaskResponseDTO>();
        }


    }
}
