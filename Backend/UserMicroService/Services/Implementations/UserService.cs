using AutoMapper;
using UserMicroService.AutoMapper;
using UserMicroService.ModelDTOs;
using UserMicroService.Services.Interfaces;
using UserMicroService.UserRepositories.Interfaces;
using UserMicroService.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using UserMicroService.UserRepositories.Implementations;


namespace UserMicroService.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper) 
        {
            _userRepository= userRepository;
            _mapper= mapper;


        }
        public async  Task<UserResponseDTO> CreateUserAsync(UserRequestDTO request)
        {
            if(request == null) throw new ArgumentNullException(nameof(request));
            var existinguser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existinguser != null)
            {

                throw new InvalidOperationException("user already exist");
               
            }
            User user = _mapper.Map<User>(request);
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user = await _userRepository.CreateUserAsync(user);
            UserResponseDTO responce = _mapper.Map<UserResponseDTO>(user);
            return responce;

        }

        public async Task<bool?> DeleteUserAsync(Guid id)
        {
            bool? status= await _userRepository.DeleteUserAsync(id);
            return status;
        }

        public async Task<UserResponseDTO?> GetUserByIdAsync(Guid id)
        {
           var user= await _userRepository.GetUserByIdAsync(id);
            UserResponseDTO response= _mapper.Map<UserResponseDTO>(user);
            return response;
        }

        public async Task<UserResponseDTO?> UpdateUserAsync(UserRequestDTO request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var user = await _userRepository.GetUserByIdAsync(request.Id);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user = await _userRepository.UpdateUserAsync(user);
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            UserResponseDTO responce = _mapper.Map<UserResponseDTO>(user);
            return responce;
        }
    }
}
