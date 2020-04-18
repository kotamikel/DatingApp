using System.Threading.Tasks;
using DatingApp.API.models;

namespace DatingApp.API.Data
{
    public interface IAuthRepository
    {
         // Register User
        Task<User> Register(User user, string password);

         // Login
        Task<User> Login(string username, string password);

         // Does User exist?
         Task<bool> UserExists(string username);
    }
}