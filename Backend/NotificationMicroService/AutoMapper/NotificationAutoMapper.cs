using AutoMapper;
using NotificationMicroService.Entities;
using NotificationMicroService.ModelDTOs;

namespace NotificationMicroService.AutoMapper
{
    public class NotificationAutoMapper :Profile
    {
     
        public NotificationAutoMapper()
        {

            CreateMap<Notification, NotificationResponseDTO>();
        }
    }
}
