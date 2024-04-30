using BookManagementAPI.Models;

namespace BookManagementAPI.Services.Repositories
{
    public interface IUserRepository
    {
        User GetUser(string username);
        void SaveNewUser(User user);
        void SaveChangedUser(User user);
        public int GetUserCount();
        public int GetRoleCount(UserRole role); //Augustas string changed to UserRole
    }
}
