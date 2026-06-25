using Microsoft.EntityFrameworkCore;
using UserMicroService.Entities;




namespace UserMicroService.Data;

    public class UserDbContext : DbContext
    {

    public UserDbContext(DbContextOptions<UserDbContext> option) : base(option) 
    {
    
    }

   public DbSet<User> Users { get; set; }

}

