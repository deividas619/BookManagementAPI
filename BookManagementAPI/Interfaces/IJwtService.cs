using BookManagementAPI.Models;

namespace BookManagementAPI.Interfaces
{
    public interface IJwtService
    {
        public string GetJwtToken(string username, UserRole role);
    }
}