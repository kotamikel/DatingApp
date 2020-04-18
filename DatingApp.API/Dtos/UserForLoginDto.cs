namespace DatingApp.API.Dtos
{
    public class UserForLoginDto
    {
        // No Validation is necessary because the API already handles this. 
        public string Username { get; set; }
        public string Password { get; set; }
    }
}