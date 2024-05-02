using System.Collections.Generic;
using System.Threading.Tasks;
using BookManagementAPI.Interfaces;
using BookManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookManagementAPI.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Admin> GetAdminAsync(string username)
        {
            return await _context.Admins.FirstOrDefaultAsync(a => a.Username == username);
        }
        public async Task<IEnumerable<Admin>> GetAllAdminsAsync()
        {
            return await _context.Admins.ToListAsync();
        }
        public async Task AddAdminAsync(Admin admin)
        {
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAdminAsync(Admin admin)
        {
            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAdminAsync(Admin admin)
        {
            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
        }
    }
}
