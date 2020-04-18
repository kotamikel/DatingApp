using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        // We are going to Pass Annotations to validate (there are many types (DataAnnotations))
        // No need to tell controller about validation(message) - automatically sent back from the "[ApiController]" attribute (AuthController.cs) 

        // Username Required Annotation
        [Required] 
        public string Username { get; set; }

        // Password Required Annotation
        [Required] 
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify the password between 4 and 8 characters.")]
        public string Password { get; set; }
    }
} 