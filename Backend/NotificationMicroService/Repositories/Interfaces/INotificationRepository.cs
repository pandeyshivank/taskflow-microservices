using NotificationMicroService.Entities;

namespace NotificationMicroService.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<Notification?> GetNotificationByIdAsync(Guid id);

        Task<List<Notification>> GetAllNotificationByUserIdAsync(Guid userId);

        Task<Notification?> CreateNotificationAsync(Notification notification);

        Task<Notification?> UpdateNotificationAsync(Notification notification);

        Task<bool> DeleteNotificationAsync(Notification notification);
    }
}
