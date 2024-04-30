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
            //string role = user.Role; //Augustas: no need
            if (user is null)
                return new ResponseDto(false, "Username or password does not match!");

            if (!VerifyPasswordHash(password, user.Password, user.PasswordSalt))
                return new ResponseDto(false, "Username or password does not match!");

            return new ResponseDto(true, "User logged in!", user.Role); //Augustas: role changed to user.Role
        }

        public ResponseDto Signup(string username, string password)
        {
            var user = _userRepository.GetUser(username);
            if (user is not null)
                return new ResponseDto(false, "User already exists!");

            user = CreateUser(username, password);
            _userRepository.SaveNewUser(user);
            return new ResponseDto(true, "User created!", user.Role); //Augustas: user.Role changed to user.Role
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

        public ResponseDto ChangeRole(string username, UserRole newRole) //Augustas: string changed to UserRole
        {
            var user = _userRepository.GetUser(username);
            if (user is null)
                return new ResponseDto(false, "User doesn't exist!");
            else
            {
                if (newRole != UserRole.Admin && _userRepository.GetRoleCount(UserRole.Admin) == 1 && user.Role == UserRole.Admin) //Augustas: "Admin" changed to UserRole.Admin
                {
                    return new ResponseDto(false, "There cannot be 0 admins!");
                }
                user.Role = newRole;
                _userRepository.SaveChangedUser(user);
                return new ResponseDto(true, "Role updated successfully!", newRole); //Augustas: added newRole
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
                Role = UserRole.Regular //Augustas: "Regular" changed to UserRole.Regular
            };

            if (_userRepository.GetUserCount() == 0)
            {
                user.Role = UserRole.Admin; //Augustas: "Admin" changed to UserRole.Admin
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
