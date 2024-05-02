using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using BookManagementAPI.Interfaces;

namespace BookManagementAPI.Services
{
    public class AdminServices : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBookRepository _bookRepository;

        public AdminServices(IAdminRepository adminRepository, IUserRepository userRepository, IBookRepository bookRepository)
        {
            _adminRepository = adminRepository;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }
        public async Task<ResponseDto> CreateAdminAsync(string username, string password)
        {
            var existingAdmin = await _adminRepository.GetAdminAsync(username);
            if (existingAdmin != null)
                return new ResponseDto(false, "Admin already exists");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var admin = new Admin
            {
                Id = Guid.NewGuid(),
                Username = username,
                Password = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _adminRepository.AddAdminAsync(admin);

            return new ResponseDto(true, "Admin created successfully");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
        public async Task<Admin> GetAdminAsync(string username)
        {
            return await _adminRepository.GetAdminAsync(username);
        }

        public async Task<ResponseDto> UpdateAdminAsync(string username)
        {
            var admin = await _adminRepository.GetAdminAsync(username);
            if (admin == null)
                return new ResponseDto(false, "Admin not found");

            await _adminRepository.UpdateAdminAsync(admin); 

            return new ResponseDto(true, "Admin updated successfully");
        }
        public async Task<ResponseDto> DeleteAdminAsync(string username)
        {
            var admin = await _adminRepository.GetAdminAsync(username);
            if (admin == null)
                return new ResponseDto(false, "Admin not found");

            await _adminRepository.DeleteAdminAsync(admin);

            return new ResponseDto(true, "Admin deleted successfully");
        }
        public async Task<ResponseDto> DeleteBookAsync(Guid bookId)
        {
            try
            {
                var book = await _bookRepository.GetBookById(bookId);
                if (book == null)
                {
                    return new ResponseDto(false, "Book not found");
                }

                await _bookRepository.RemoveBookById(bookId);
                return new ResponseDto(true, "Book deleted successfully");
            }
            catch (Exception ex)
            {
                return new ResponseDto(false, $"Failed to delete book: {ex.Message}");
            }
        }
        public async Task<ResponseDto> AddBookAsync(BookDto bookDto)
        {
            try
            {
                var book = new Book
                {
                    Id = Guid.NewGuid(),
                    Title = bookDto.Title,
                    Author = bookDto.Author,
                    Publication = bookDto.Publication,
                };
                var addedBook = await _bookRepository.AddBook(book);

                if (addedBook == null)
                {
                    return new ResponseDto(false, "Failed to add book");
                }

                return new ResponseDto(true, "Book added successfully");
            }
            catch (Exception ex)
            {
                return new ResponseDto(false, ex.Message);
            }
        }
        public async Task<ResponseDto> SetAdminAndChangeRoleAsync(string username, string newRole)
        {
            var user = await _userRepository.GetUserAsync(username);
            if (user == null)
                return new ResponseDto(false, "User not found");

            user.Role = UserRole.Admin; 
            _userRepository.SaveChangedUser(user);

            return new ResponseDto(true, "User role changed to Admin successfully");
        }

        public async Task<ResponseDto> RevokeAdminAndChangeRoleAsync(string username, string newRole)
        {
            var user = await _userRepository.GetUserAsync(username);
            if (user == null)
                return new ResponseDto(false, "User not found");

            user.Role = UserRole.Regular; 
            _userRepository.SaveChangedUser(user);

            return new ResponseDto(true, "User role changed to Regular successfully");
        }

    }
}
