using System;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookManagementAPI.Interfaces;
using BookManagementAPI.Models;

namespace BookManagementAPI.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetJwtToken(string username, UserRole role)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role.ToString())
            };

            var secretToken = _configuration.GetSection("Jwt:Key").Value;
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretToken));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var token = new JwtSecurityToken
            (
                issuer: _configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}