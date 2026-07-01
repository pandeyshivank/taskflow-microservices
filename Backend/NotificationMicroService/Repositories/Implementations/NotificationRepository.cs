using Microsoft.EntityFrameworkCore;
using NotificationMicroService.Data;
using NotificationMicroService.Entities;
using NotificationMicroService.Repositories.Interfaces;

namespace NotificationMicroService.Repositories.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationDbContext _context;
        public NotificationRepository(NotificationDbContext context) { 
            _context = context;
        }
        public async Task<Notification?> CreateNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> DeleteNotificationAsync(Notification notification)
        {
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Notification>> GetAllNotificationByUserIdAsync(Guid userId)
        {
            var resultList = await _context.Notifications.Where(item => item.UserId == userId).ToListAsync();

            return resultList;
        }

        public async Task<Notification?> GetNotificationByIdAsync(Guid id)
        {
            var notification = await _context.Notifications.FindAsync(id);

            return notification;
        }

        public async Task<Notification?> UpdateNotificationAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return notification;
        }
    }
}
