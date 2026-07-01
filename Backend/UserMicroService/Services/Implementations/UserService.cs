using AutoMapper;
using UserMicroService.AutoMapper;
using UserMicroService.ModelDTOs;
using UserMicroService.Services.Interfaces;
using UserMicroService.UserRepositories.Interfaces;
using UserMicroService.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using UserMicroService.UserRepositories.Implementations;
using System.Diagnostics.Eventing.Reader;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure;


namespace UserMicroService.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _Configuration;
        public UserService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration) 
        {
            _userRepository= userRepository;
            _mapper= mapper;
            _Configuration= configuration;


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
        public async Task<LoginResponseDTO?> loginAsync(LoginRequestDTO request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null) throw new InvalidOperationException("user Not exist");
           
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!isPasswordValid)
            {
                throw new InvalidOperationException("Wrong Password");
                
            }
            LoginResponseDTO responce = new LoginResponseDTO()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId=user.Id


            };
            DateTime expiry = DateTime.UtcNow.AddHours(4);
            responce.ExpiresAt = expiry;
            responce.Token = GenerateToken(user);
            return responce;

        }
        public string GenerateToken(User loginuser)
        {
            var key = _Configuration.GetValue<string>("JwtTokenKey");

            var handler = new JwtSecurityTokenHandler();
            var TokenDescriptor = new SecurityTokenDescriptor()
            {
              Subject = new ClaimsIdentity(new Claim[]
              {
                  new Claim(ClaimTypes.Name, loginuser.FirstName),
                  new Claim(ClaimTypes.Role, "Admin"),
                  new Claim(ClaimTypes.NameIdentifier, loginuser.Id.ToString()),
                  new Claim(JwtRegisteredClaimNames.Sub, loginuser.Id.ToString()),
                  new Claim(JwtRegisteredClaimNames.Email, loginuser.Email),
                  



              }),
              Expires = DateTime.UtcNow.AddHours(4),
              SigningCredentials= new (new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),SecurityAlgorithms.HmacSha256),
             
            };
            var securityToken =  handler.CreateToken(TokenDescriptor);
            var Token= handler.WriteToken(securityToken);
            return Token.ToString();
           
        }

    }
}
