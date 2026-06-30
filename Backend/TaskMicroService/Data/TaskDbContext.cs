using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TaskMicroService.Entities;
namespace TaskMicroService.Data

{
    public class TaskDbContext : DbContext
    {

        public TaskDbContext (DbContextOptions<TaskDbContext> option) : base (option)
        {


        }
       public DbSet<TaskItem> Tasks { get; set; }
    }
}
