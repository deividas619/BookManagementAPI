using BookManagementAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookManagementAPI.Services.Repositories
{
    public interface IAdminRepository
    {
        Task<Admin> GetAdminAsync(string username);
        Task AddAdminAsync(Admin admin);
        Task UpdateAdminAsync(Admin admin);
        Task DeleteAdminAsync(Admin admin);

    }
}
