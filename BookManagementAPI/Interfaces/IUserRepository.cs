using System.Threading.Tasks;
using BookManagementAPI.Models;

namespace BookManagementAPI.Interfaces
{
    public interface IUserRepository
    {
        User GetUser(string username);
        void SaveNewUser(User user);
        void SaveChangedUser(User user);
        public int GetRoleCount(UserRole role);
        Task<User> GetUserAsync(string username);
    }
}