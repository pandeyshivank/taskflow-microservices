
using UserMicroService.Entities;
using UserMicroService.ModelDTOs;
using UserMicroService.UserRepositories;
namespace UserMicroService.Services.Interfaces

{
    public interface IUserService
    {
        public Task<UserResponseDTO> CreateUserAsync(UserRequestDTO request);
        public Task<UserResponseDTO?> UpdateUserAsync(UserRequestDTO request);
        public Task<bool?> DeleteUserAsync(Guid id);

        public Task<UserResponseDTO?> GetUserByIdAsync(Guid id);
        public Task<LoginResponseDTO?> loginAsync(LoginRequestDTO request);

    }
}
