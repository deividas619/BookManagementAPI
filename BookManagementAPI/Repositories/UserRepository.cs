using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementAPI.Interfaces;
using BookManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagementAPI.Repositories
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

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<List<Review>> GetReviewsByUserIdAsync(Guid userId)
        {
            return await _context.Reviews.Where(r => r.Id == userId).ToListAsync();
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

        public int GetRoleCount(UserRole role)
        {
            return _context.Users.Count(u => u.Role == role);
        }

        public async Task<User> GetUserAsync(string username)
        {
            var result = await _context.Users.SingleOrDefaultAsync(x => x.Username == username);
            return result;
        }
    }
}