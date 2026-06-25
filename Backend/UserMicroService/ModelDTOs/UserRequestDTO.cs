using System.ComponentModel.DataAnnotations;

namespace UserMicroService.ModelDTOs
{
    public class UserRequestDTO
    {
        public Guid Id { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
