using NotificationMicroService.Entities;
using NotificationMicroService.ModelDTOs;

namespace NotificationMicroService.Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationResponseDTO>> GetAllNotificationByUserIdAsync(Guid userId);

        Task<NotificationResponseDTO?> GetNotificationByIdAsync(Guid id);

        Task<bool> DeleteNotificationAsync(Guid id);

        Task<bool> MarkAsReadAsync(Guid id);

       
        Task<NotificationResponseDTO> CreateNotificationAsync(Notification notification);
        
    }
}
