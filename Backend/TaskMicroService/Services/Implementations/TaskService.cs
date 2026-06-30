using AutoMapper;
using Azure.Core;
using TaskMicroService.Entities;
using TaskMicroService.ModelDTOs;
using TaskMicroService.Services.Interfaces;
using TaskMicroService.TaskRepositories.Interfaces;

namespace TaskMicroService.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ITaskRepository _taskRepository;

        public TaskService(IMapper mapper, IConfiguration configuration, ITaskRepository taskRepository)
        {
            _mapper = mapper;
            _configuration = configuration;
            _taskRepository = taskRepository;

        }
        public async Task<TaskResponseDTO> CreateTaskAsync(TaskRequestDTO request)
        {
            if (request == null) { throw new InvalidOperationException("please enter task data"); }
            var taskitem=   _mapper.Map<TaskItem>(request);
            var responce= await _taskRepository.CreateTaskAsync(taskitem);
            if (responce == null) {
                throw new NullReferenceException(); 
            }
            return _mapper.Map<TaskResponseDTO>(responce); 


        }

        public async Task<bool> DeleteTaskAsync(Guid id)
        {
            if (id == Guid.Empty) { throw new NullReferenceException(); }

            var existingtask = await _taskRepository.GetTaskByIdAsync(id);
            if (existingtask == null)
            {
                throw new NullReferenceException();
            }
           
            var responce = await _taskRepository.DeleteTaskAsync(existingtask);
            if (responce == null)
            {
                throw new InvalidOperationException("No record present for this id");
            }
            return true;
        }

        public async Task<List<TaskResponseDTO>> GetAllTaskByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty) { throw new InvalidOperationException("please provide valid userid"); }

            var existingtask = await _taskRepository.GetAllTaskByUserIdAsync(userId);
            if (existingtask.Count == 0)
            {
                throw new InvalidOperationException("No Task present for this user");
            }
            List< TaskResponseDTO > responce = new List< TaskResponseDTO >();
           
                responce= _mapper.Map<List<TaskResponseDTO>>(existingtask);
            
            return responce;
        }

        public async Task<TaskResponseDTO?> GetTaskByIdAsync(Guid id)
        {
            if (id == Guid.Empty) { throw new NullReferenceException(); }

            var existingtask = await _taskRepository.GetTaskByIdAsync(id);
            if (existingtask == null)
            {
                throw new InvalidOperationException("please provide valid task id");
            }

            return _mapper.Map<TaskResponseDTO>(existingtask);
             
        }

        public async Task<TaskResponseDTO> UpdateTaskAsync(TaskRequestDTO request)
        {
            if (request == null) { throw new NullReferenceException(); }

            var existingtask= await _taskRepository.GetTaskByIdAsync(request.Id);
            if (existingtask == null)
            {
                throw new InvalidOperationException("Task not found");
            }
            existingtask.Title=request.Title;
            existingtask.Description=request.Description;
            existingtask.Status=request.Status;
            existingtask.DueDate=request.DueDate;
            existingtask.UserId=request.UserId;
            existingtask.UpdatedAt = DateTime.UtcNow;
            var responce = await _taskRepository.UpdateTaskAsync(existingtask);
            if (responce == null)
            {
                throw new InvalidOperationException("Task not found");
            }
            return _mapper.Map<TaskResponseDTO>(responce);

        }
    }
}
