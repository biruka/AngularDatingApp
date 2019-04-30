using System.Threading.Tasks;
using DatingApp.API.Models;
namespace DatingApp.API.Data
{
    public interface IAuthRepositary
    {
         Task<User> Register (User user, string password);

         Task<User> Login(string user, string password);

         Task<bool> UserExists(string username);
     }
}