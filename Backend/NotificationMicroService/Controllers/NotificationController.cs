using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationMicroService.Entities;
using NotificationMicroService.ModelDTOs;
using NotificationMicroService.Services.Implementations;
using NotificationMicroService.Services.Interfaces;
namespace NotificationMicroService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpPost("CreateNotification")]
        [ProducesResponseType(typeof(NotificationResponseDTO), 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]


        public async Task<ActionResult<NotificationResponseDTO>> CreateNotificationAsync([FromBody] Notification request)
        {
            
            return Ok(await _notificationService.CreateNotificationAsync(request));
        }
        [HttpPut("MarkAsRead/{id:guid}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> MarkAsReadAsync([FromRoute] Guid id)
        {
            return Ok(await _notificationService.MarkAsReadAsync(id));
        }

        [HttpDelete("DeleteNotification/{id:guid}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<bool> DeleteNotificationAsync([FromRoute] Guid id)
        {
            return await _notificationService.DeleteNotificationAsync(id);
        }

        [HttpGet("GetNotification/{id:guid}")]
        [ProducesResponseType(typeof(NotificationResponseDTO), 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<NotificationResponseDTO>> GetNotificationByIdAsync([FromRoute] Guid id)
        {
            var responce = await _notificationService.GetNotificationByIdAsync(id);
            return Ok(responce);
        }

        [HttpGet("GetAllUserNotification/{userId:guid}")]
        [ProducesResponseType(typeof(List<NotificationResponseDTO>), 200)]
        [ProducesResponseType(statusCode: 400)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<NotificationResponseDTO>>> GetAllNotificationByUserIdAsync([FromRoute] Guid userId)
        {
            return Ok(await _notificationService.GetAllNotificationByUserIdAsync(userId));
        }
    }
}

