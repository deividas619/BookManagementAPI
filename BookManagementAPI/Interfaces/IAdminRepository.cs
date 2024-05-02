using System.Threading.Tasks;
using BookManagementAPI.Models;

namespace BookManagementAPI.Interfaces
{
    public interface IAdminRepository
    {
        Task<Admin> GetAdminAsync(string username);
        Task AddAdminAsync(Admin admin);
        Task UpdateAdminAsync(Admin admin);
        Task DeleteAdminAsync(Admin admin);

    }
}
