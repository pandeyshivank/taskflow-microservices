using TaskMicroService.Entities;
using System.Collections.Generic;


namespace TaskMicroService.TaskRepositories.Interfaces
{
    public interface ITaskRepository
    {

        Task<TaskItem?> CreateTaskAsync(TaskItem item);

        Task<TaskItem?> UpdateTaskAsync(TaskItem item);

        Task<TaskItem?> DeleteTaskAsync(TaskItem item);

        Task<List<TaskItem>> GetAllTaskByUserIdAsync(Guid id);

        Task<TaskItem?> GetTaskByIdAsync(Guid id);
    }
}
