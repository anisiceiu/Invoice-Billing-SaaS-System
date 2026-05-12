using InvoiceSystem.Application.Dtos;
using InvoiceSystem.Application.Interfaces;
using InvoiceSystem.Application.Interfaces.Services;
using InvoiceSystem.Application.Utils;
using InvoiceSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceSystem.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AccountService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            return user;
        }

        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            var exists = await _userRepository.IsEmailRegisteredAsync(email);

            return exists;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await GetUserByEmail(loginDto.Email);
            
            var token = GenerateJwtToken(user);
            var response = new AuthResponseDto
            {
                Token = token,
                UserName = user!.Username,
                Email = user!.Email,
                Role = user!.Role
            };

            return response;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            

            var user = new User
            {
                Username = dto.UserName,
                Email = dto.Email,
                PasswordHash = PasswordHasher.HashPassword(dto.Password),
                //Phone = dto.Phone,
                Role = dto.Role,
                //IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
        
            
            var token = GenerateJwtToken(user);
            var response = new AuthResponseDto
            {
                Token = token,
                UserName = user.Username,
                Email = user.Email,
                Role = user.Role
            };


            return response;

        }

        public async Task<bool> VerifyPassword(string passwordHash,string password)
        {
            return PasswordHasher.VerifyPassword(passwordHash, password);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSection = _config.GetSection("Jwt");
            var key = jwtSection.GetValue<string>("Key");
            var issuer = jwtSection.GetValue<string>("Issuer");
            var audience = jwtSection.GetValue<string>("Audience");
            var expireMinutes = jwtSection.GetValue<int>("ExpireMinutes");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("username", user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var creds = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }


}
