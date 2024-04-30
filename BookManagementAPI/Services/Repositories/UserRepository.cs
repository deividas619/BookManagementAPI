using System.Linq;
using BookManagementAPI.Models;

namespace BookManagementAPI.Services.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public User GetUser(string username)
        {
            var users = _context.Users;
            var result =  _context.Users.SingleOrDefault(x => x.Username == username);
            return _context.Users.SingleOrDefault(x => x.Username == username);
        }

        public void SaveNewUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        
        public void SaveChangedUser(User user)
        {
            _context.SaveChanges();
        }

        public int GetUserCount()
        {
            return _context.Users.Count();
        }

        public int GetRoleCount(UserRole role) //Augustas: string changed to UserRole
        {
            return _context.Users.Count(u => u.Role == role);
        }
    }
}
