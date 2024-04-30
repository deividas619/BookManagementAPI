using BookManagementAPI.DTOs;
using BookManagementAPI.Models;

namespace BookManagementAPI.Services
{
    public interface IUserService
    {
        ResponseDto Signup(string username, string password);
        ResponseDto Login(string username, string password);
        ResponseDto ChangePassword(string username, string oldPassword, string newPassword, string newPasswordAgain);
        ResponseDto ChangeRole(string username, UserRole newRole); //Augustas: string changed to UserRole
    }
}
