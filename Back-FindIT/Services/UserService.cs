using Back_FindIT.Data;
using Back_FindIT.Dtos.UserDtos;
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

        public async Task<UserReturnDto?> GetUserByIdAsync(int id)
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

        public async Task<ICollection<UserReturnDto>> GetUsersAsync()
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

        public async Task<bool> SoftDeleteUserAsync(int userId)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return false;

            if (!user.IsActive)
                return true;


            user.IsActive = false;
            user.SetUpdatedAt();

            _appDbContext.Users.Update(user);
            await _appDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<UserReturnDto?> UpdateUserAsync(int id, UserUpdateDto userDto)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return null;

            if (!user.IsActive)
                throw new UnauthorizedAccessException("Usuário desativado.");

            if (await _appDbContext.Users.AnyAsync(u => u.Email == userDto.Email && u.Id != id))
                throw new InvalidOperationException("Já existe um usuário cadastrado com esse e-mail.");

            if (await _appDbContext.Users.AnyAsync(u => u.Cpf == userDto.Cpf && u.Id != id))
                throw new InvalidOperationException("Já existe um usuário cadastrado com esse CPF.");

            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.Cpf = userDto.Cpf;
            user.SetUpdatedAt();

            _appDbContext.Users.Update(user);
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
    }
}
