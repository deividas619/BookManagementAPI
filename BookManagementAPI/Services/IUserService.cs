using BookManagementAPI.DTOs;

namespace BookManagementAPI.Services
{
    public interface IUserService
    {
        ResponseDto Signup(string username, string password);
        ResponseDto Login(string username, string password);
        ResponseDto ChangePassword(string username, string oldPassword, string newPassword, string newPasswordAgain);
        ResponseDto ChangeRole(string username, string newRole);
    }
}
