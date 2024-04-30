using BookManagementAPI.Models;

namespace BookManagementAPI.Services
{
    public interface IJwtService
    {
        public string GetJwtToken(string username, UserRole role);
    }
}
