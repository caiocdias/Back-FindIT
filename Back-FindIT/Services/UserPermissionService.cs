using Back_FindIT.Data;
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
    }

}
