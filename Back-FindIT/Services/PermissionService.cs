using Back_FindIT.Data;
using Back_FindIT.Dtos.Permission;
using Back_FindIT.Dtos.User;
using Back_FindIT.Models;
using Microsoft.EntityFrameworkCore;

namespace Back_FindIT.Services
{
    public class PermissionService
    {
        private readonly AppDbContext _appDbContext;

        public PermissionService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Permission?> AddPermissionAsync(PermissionRegisterDto permissionDto)
        {
            if (await _appDbContext.Permissions.AnyAsync(p => p.PermissionKey == permissionDto.PermissionKey))
                throw new InvalidOperationException("Esta Permission Key já está cadastrada");

            var permission = new Permission
            {
                PermissionKey = permissionDto.PermissionKey,
                Description = permissionDto.Description
            };

            permission.SetUpdatedAt();

            _appDbContext.Permissions.Add(permission);
            await _appDbContext.SaveChangesAsync();

            return permission;
        }

        public async Task<PermissionReturnDto?> GetPermissionByIdAsync(int id)
        {
            var permission = await _appDbContext.Permissions
                .Include(p => p.UserPermissions)
                .ThenInclude(up => up.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (permission == null)
                return null;

            return new PermissionReturnDto
            {
                Id = permission.Id,
                PermissionKey = permission.PermissionKey,
                Description = permission.Description,
                Users = permission.UserPermissions
                    .Select(up => new UserDto
                    {
                        Id = up.User.Id,
                        Name = up.User.Name,
                        Email = up.User.Email
                    }).ToList()
            };
        }
    }
}
