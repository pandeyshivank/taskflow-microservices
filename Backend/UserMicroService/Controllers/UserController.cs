using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserMicroService.Services;
using UserMicroService.ModelDTOs;
using UserMicroService.Services.Interfaces;
using Azure;
namespace UserMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) {
            _userService=userService;
        }


        [Route("Create",Name ="CreateUser")]
        [HttpPost]
        [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UserResponseDTO>> CreateUser(UserRequestDTO request) {

            var responce= await _userService.CreateUserAsync(request);
            return Ok(responce);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UserResponseDTO>> GetUserById(Guid id)
        {
            var user =await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [ProducesResponseType(typeof(UserResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Route("Update", Name = "UpdateUser")]
        [HttpPut]
        public async Task<ActionResult<UserResponseDTO>> UpdateUser(UserRequestDTO request)
        {

            var responce = await _userService.UpdateUserAsync(request);
            return Ok(responce);
        }

        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [HttpDelete]
        [Route("Delete/{id}", Name ="DeleteUser")]
        public async Task<bool?> DeleteUser(Guid id)
        {
            bool? status= await _userService.DeleteUserAsync(id);
            return status;

        }
    }
}
