using System;
using System.Threading.Tasks;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;

namespace BookManagementAPI.Interfaces
{
    public interface IAdminService
    {
        Task<ResponseDto> CreateAdminAsync(string username, string password);
        Task<Admin> GetAdminAsync(string username);
        Task<ResponseDto> UpdateAdminAsync(string username);
        Task<ResponseDto> DeleteAdminAsync(string username);
        Task<ResponseDto> DeleteBookAsync(Guid bookId);
        Task<ResponseDto> AddBookAsync(BookDto bookDto);
        Task<ResponseDto> SetAdminAndChangeRoleAsync(string username, string newRole);
        Task<ResponseDto> RevokeAdminAndChangeRoleAsync(string username, string newRole);
    }
}