using BookManagementAPI.Models;
using System.Threading.Tasks;

namespace BookManagementAPI.Services.Repositories
{
    public interface IUserRepository
    {
        User GetUser(string username);
        void SaveNewUser(User user);
        void SaveChangedUser(User user);
        public int GetUserCount();
        public int GetRoleCount(string role);
        Task<User> GetUserAsync(string username);
    }
}
