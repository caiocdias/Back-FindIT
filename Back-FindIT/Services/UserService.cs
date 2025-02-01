using Back_FindIT.Data;
using Back_FindIT.Dtos;
using Back_FindIT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Back_FindIT.Services
{
    public class UserService
    {
        private readonly AppDbContext _appDbContext;

        public UserService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<UserReturnDto?> AddUserAsync(UserRegisterDto userDto)
        {
            // Verifica se o e-mail já existe
            if (await _appDbContext.Users.AnyAsync(u => u.Email == userDto.Email))
                throw new InvalidOperationException("Já existe um usuário cadastrado com esse e-mail.");

            // Verifica se o CPF já existe
            if (await _appDbContext.Users.AnyAsync(u => u.Cpf == userDto.Cpf))
                throw new InvalidOperationException("Já existe um usuário cadastrado com esse CPF.");

            var user = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Cpf = userDto.Cpf,
                IsActive = true
            };

            user.SetPassword(userDto.Password);
            user.SetUpdatedAt();

            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();

            return new UserReturnDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Cpf = user.Cpf,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                UserPermissions = user.UserPermissions
            };
        }

        public async Task<UserReturnDto?> GetUserById(int id)
        {
            var user = await _appDbContext.Users
                .Include(u => u.UserPermissions)
                .ThenInclude(up => up.Permission)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            return new UserReturnDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Cpf = user.Cpf,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                UserPermissions = user.UserPermissions
            };
        }

        public async Task<UserReturnDto?> GetUserByEmailAsync(string email)
        {
            var user = await _appDbContext.Users
                .Include(u => u.UserPermissions)
                    .ThenInclude(up => up.Permission)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return null;

            return new UserReturnDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Cpf = user.Cpf,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                UserPermissions = user.UserPermissions
            };
        }

        public async Task<ICollection<UserReturnDto>> GetUsers()
        {
            var users = await _appDbContext.Users
                .Where(u => u.IsActive)
                .Include(u => u.UserPermissions)
                    .ThenInclude(up => up.Permission)
                .AsNoTracking()
                .ToListAsync();

            return users.Select(user => new UserReturnDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Cpf = user.Cpf,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                UserPermissions = user.UserPermissions
            }).ToList();
        }
    }
}
