using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TaskMicroService.Data;
using TaskMicroService.Entities;
using TaskMicroService.TaskRepositories.Interfaces;
using System.Linq;

namespace TaskMicroService.TaskRepositories.Implementations
{
    public class TaskRepository :ITaskRepository
    {
        private readonly TaskDbContext _dbContext;
        public TaskRepository(TaskDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TaskItem?> CreateTaskAsync(TaskItem item)
        {
           _dbContext.Tasks.Add(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<TaskItem?> DeleteTaskAsync(TaskItem item)
        {
            _dbContext.Tasks.Remove(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<List<TaskItem>> GetAllTaskByUserIdAsync(Guid id)
        {
        var resultList = await _dbContext.Tasks.Where(item => item.UserId==id).ToListAsync();
           
            return resultList;
        }  

        public async Task<TaskItem?> GetTaskByIdAsync(Guid id)
        {
           var taskitem = await _dbContext.Tasks.FindAsync(id);

            return taskitem;
        }

        public async Task<TaskItem?> UpdateTaskAsync(TaskItem item)
        {
           _dbContext.Tasks.Update(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }
    }
}
