using AutoMapper;
using NotificationMicroService.Entities;
using NotificationMicroService.ModelDTOs;
using NotificationMicroService.Services.Interfaces;
using NotificationMicroService.Repositories.Interfaces;
using Azure.Core;
using Microsoft.EntityFrameworkCore;

namespace NotificationMicroService.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;
        private readonly INotificationRepository _repository;

        public NotificationService(IMapper mapper,  INotificationRepository notificationRepository)
        {
            _mapper = mapper;
            _repository = notificationRepository;

        }
        public async Task<NotificationResponseDTO> CreateNotificationAsync(Notification notification)
        {
            if (notification == null) { throw new InvalidOperationException("please enter valid notification"); }
          
            var responce = await _repository.CreateNotificationAsync(notification);
            if (responce == null)
            {
                throw new NullReferenceException();
            }
            return _mapper.Map<NotificationResponseDTO>(responce);
        }

        public async Task<bool> DeleteNotificationAsync(Guid id)
        {
            if (id == Guid.Empty) { throw new NullReferenceException(); }

            var existingNotification = await _repository.GetNotificationByIdAsync(id);
            if (existingNotification == null)
            {
                throw new NullReferenceException();
            }

            var responce = await _repository.DeleteNotificationAsync(existingNotification);
            if (responce == null)
            {
                throw new InvalidOperationException("No record present for this id");
            }
            return true;
        }

        public async Task<List<NotificationResponseDTO>> GetAllNotificationByUserIdAsync(Guid userId)
        {
            if (userId == Guid.Empty) { throw new InvalidOperationException("please provide valid userid"); }

            var existingNotification = await _repository.GetAllNotificationByUserIdAsync(userId);
            if (existingNotification.Count == 0)
            {
                throw new InvalidOperationException("No Notification  present for this user");
            }
            List<NotificationResponseDTO> responce = new List<NotificationResponseDTO>();

            responce = _mapper.Map<List<NotificationResponseDTO>>(existingNotification);

            return responce;
        }

            public async Task<NotificationResponseDTO?> GetNotificationByIdAsync(Guid id)
        {
            if (id == Guid.Empty) { throw new NullReferenceException(); }

            var existingNotification = await _repository.GetNotificationByIdAsync(id);
            if (existingNotification == null)
            {
                throw new InvalidOperationException("please provide valid task id");
            }

            return _mapper.Map<NotificationResponseDTO>(existingNotification);
        }

       

        public async Task<bool> MarkAsReadAsync(Guid id)
        {
            if (id == Guid.Empty) { throw new NullReferenceException(); }

            var existingNotification = await _repository.GetNotificationByIdAsync(id);
            if (existingNotification == null)
            {
                throw new InvalidOperationException("Notification  not found");
            }
           

            existingNotification.ReadAt = DateTime.UtcNow;
            existingNotification.IsRead= true;

            var responce = await _repository.UpdateNotificationAsync(existingNotification);
            if (responce == null)
            {
                throw new InvalidOperationException("Notification  not found");
            }
            return true;
        }
    }
}
