using System;
using System.Linq;
using System.Security.Cryptography;
using BookManagementAPI.DTOs;
using BookManagementAPI.Interfaces;
using BookManagementAPI.Models;

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
            if (user is null)
                return new ResponseDto(false, "Username does not exist!");

            if (!VerifyPasswordHash(password, user.Password, user.PasswordSalt))
                return new ResponseDto(false, "Password is incorrect!");

            return new ResponseDto(true, "User logged in!", user.Role);
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

        public ResponseDto ChangeRole(string username, UserRole newRole)
        {
            var user = _userRepository.GetUser(username);
            if (user is null)
                return new ResponseDto(false, "User doesn't exist!");
            else
            {
                if (newRole != UserRole.Admin && _userRepository.GetRoleCount(UserRole.Admin) == 1 && user.Role == UserRole.Admin)
                {
                    return new ResponseDto(false, "There cannot be 0 admins!");
                }
                else if (newRole == user.Role)
                {
                    return new ResponseDto(false, "User already has that role!");
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
                Role = UserRole.Regular
            };

            if (string.Equals(username, "admin", StringComparison.OrdinalIgnoreCase))
            {
                user.Role = UserRole.Admin;
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