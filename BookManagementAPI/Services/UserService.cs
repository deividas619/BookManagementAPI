using System;
using System.Linq;
using System.Security.Cryptography;
using BookManagementAPI.DTOs;
using BookManagementAPI.Models;
using BookManagementAPI.Services.Repositories;

namespace BookManagementAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository repository)
        {
            _userRepository = repository;
        }

        public ResponseDto Login(string username, string password)
        {
            var user = _userRepository.GetUser(username);
            string role = user.Role;
            if (user is null)
                return new ResponseDto(false, "Username or password does not match!");

            if (!VerifyPasswordHash(password, user.Password, user.PasswordSalt))
                return new ResponseDto(false, "Username or password does not match!");

            return new ResponseDto(true, "User logged in!", role);
        }

        public ResponseDto Signup(string username, string password)
        {
            var user = _userRepository.GetUser(username);
            if (user is not null)
                return new ResponseDto(false, "User already exists!");

            user = CreateUser(username, password);
            _userRepository.SaveNewUser(user);
            return new ResponseDto(true, "User created!", user.Role);
        }

        public ResponseDto ChangePassword(string username, string oldPassword, string newPassword, string newPasswordAgain)
        {
            var user = _userRepository.GetUser(username);
            if (VerifyPasswordHash(oldPassword, user.Password, user.PasswordSalt))
            {
                if (newPassword == newPasswordAgain)
                {
                    CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
                    user.Password = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    _userRepository.SaveChangedUser(user);
                    return new ResponseDto(true, "Password updated!");
                }
                return new ResponseDto(false, "Passwords are different!");
            }
            return new ResponseDto(false, "Old password is incorrect!");
        }

        public ResponseDto ChangeRole(string username, string newRole)
        {
            var user = _userRepository.GetUser(username);
            if (user is null)
                return new ResponseDto(false, "User doesn't exist!");
            else
            {
                if (newRole != "Admin" && _userRepository.GetRoleCount("Admin") == 1 && user.Role == "Admin")
                {
                    return new ResponseDto(false, "There cannot be 0 admins!");
                }
                user.Role = newRole;
                _userRepository.SaveChangedUser(user);
                return new ResponseDto(true, "Role updated successfully!", newRole);
            }
        }

        private User CreateUser(string username, string password)
        {
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Password = passwordHash,
                PasswordSalt = passwordSalt,
                Role = "Regular"
            };

            if (string.Equals(username, "admin", StringComparison.OrdinalIgnoreCase))
            {
                user.Role = "Admin";
            }

            return user;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
