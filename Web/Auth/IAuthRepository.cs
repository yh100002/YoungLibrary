using System.Threading.Tasks;
using Models;

namespace Web.Auth
{
    public interface IAuthRepository
    {
         Task<User> Register(User passenger, string password);
         Task<User> Login(string passengerName, string password);
         Task<bool> PassengerExists(string passengerName);
    }
}