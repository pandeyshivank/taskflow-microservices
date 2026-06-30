using TaskMicroService.ModelDTOs;

namespace TaskMicroService.Services.Interfaces
{
    public interface ITaskService
    {
        Task<TaskResponseDTO> CreateTaskAsync(TaskRequestDTO request);

        Task<TaskResponseDTO> UpdateTaskAsync(TaskRequestDTO request);

        Task<bool> DeleteTaskAsync(Guid id);

        Task<TaskResponseDTO?> GetTaskByIdAsync(Guid id);

        Task<List<TaskResponseDTO>> GetAllTaskByUserIdAsync(Guid userId);
    }
}
