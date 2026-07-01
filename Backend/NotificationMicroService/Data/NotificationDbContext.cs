using Microsoft.EntityFrameworkCore;
using NotificationMicroService.Entities;

namespace NotificationMicroService.Data
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext (DbContextOptions<NotificationDbContext> options): base(options)
        {

        }
        public DbSet<Notification> Notifications { get; set; }
    }
}
