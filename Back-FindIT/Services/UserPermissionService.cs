using Back_FindIT.Data;
using Back_FindIT.Dtos.PermissionDtos;
using Back_FindIT.Dtos.UserDtos;
using Back_FindIT.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_FindIT.Services
{
    public class UserPermissionService
    {
        private readonly AppDbContext _appDbContext;

        public UserPermissionService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> AddUserPermissionAsync(int userId, int permissionId)
        {
            var user = await _appDbContext.Users.FindAsync(userId);
            var permission = await _appDbContext.Permissions.FindAsync(permissionId);

            if (user == null || permission == null)
                throw new InvalidOperationException("Usuário ou permissão não encontrados.");

            bool alreadyExists = await _appDbContext.UserPermissions
                .AnyAsync(up => up.UserId == userId && up.PermissionId == permissionId);

            if (alreadyExists)
                throw new InvalidOperationException("Usuário já possui essa permissão.");

            var userPermission = new UserPermission
            {
                UserId = userId,
                PermissionId = permissionId,
                IsActive = true
            };

            _appDbContext.UserPermissions.Add(userPermission);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserPermissionAsync(int userId, int permissionId)
        {
            var userPermission = await _appDbContext.UserPermissions
                .FirstOrDefaultAsync(up => up.UserId == userId && up.PermissionId == permissionId);

            if (userPermission == null)
                return false;

            _appDbContext.UserPermissions.Remove(userPermission);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<PermissionReturnDto>> GetPermissionsByUserAsync(int userId)
        {
            var userPermissions = await _appDbContext.UserPermissions
                .Where(up => up.UserId == userId)
                .Include(up => up.Permission)
                .AsNoTracking()
                .ToListAsync();

            return userPermissions.Select(up => new PermissionReturnDto
            {
                Id = up.Permission.Id,
                PermissionKey = up.Permission.PermissionKey,
                Description = up.Permission.Description
            }).ToList();
        }

        public async Task<List<UserReturnDto>> GetUsersByPermissionAsync(int permissionId)
        {
            var userPermissions = await _appDbContext.UserPermissions
                .Where(up => up.PermissionId == permissionId)
                .Include(up => up.User)
                .AsNoTracking()
                .ToListAsync();

            return userPermissions.Select(up => new UserReturnDto
            {
                Id = up.User.Id,
                Name = up.User.Name,
                Email = up.User.Email,
                Cpf = up.User.Cpf,
                IsActive = up.User.IsActive,
                CreatedAt = up.User.CreatedAt,
                UpdatedAt = up.User.UpdatedAt
            }).ToList();
        }


    }

}
