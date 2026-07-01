using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskMicroService.ModelDTOs;
using TaskMicroService.Services.Interfaces;

namespace TaskMicroService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService) { 
         _taskService = taskService;
        }

        [HttpPost("CreateTask")]
        [ProducesResponseType(typeof(TaskResponseDTO),200)]
        [ProducesResponseType(statusCode:400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]


        public async Task<ActionResult<TaskResponseDTO>> CreateTaskAsync([FromBody] TaskRequestDTO request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok( await _taskService.CreateTaskAsync(request));
        }
        [HttpPut("UpdateTask")]
        [ProducesResponseType(typeof(TaskResponseDTO), 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskResponseDTO>> UpdateTaskAsync([FromBody] TaskRequestDTO request)
        {
            return Ok(await _taskService.UpdateTaskAsync(request));
        }
        [HttpDelete("DeleteTask/{id:guid}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async  Task<bool> DeleteTaskAsync([FromRoute] Guid id)
        {
            return await _taskService.DeleteTaskAsync(id);
        }

        [HttpGet("GetTask/{id:guid}")]
        [ProducesResponseType(typeof(TaskResponseDTO), 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public  async Task<ActionResult<TaskResponseDTO>> GetTaskByIdAsync([FromRoute] Guid id)
        {
             var responce= await _taskService.GetTaskByIdAsync(id);
            return Ok(responce);
        }

        [HttpGet("GetAllUserTask/{userId:guid}")]
        [ProducesResponseType(typeof(List<TaskResponseDTO>), 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<TaskResponseDTO>>> GetAllTaskByUserIdAsync([FromRoute] Guid userId)
        {
         return Ok( await _taskService.GetAllTaskByUserIdAsync(userId));
        }
    }
}
