﻿using Back_FindIT.Dtos.User;

namespace Back_FindIT.Dtos.Permission
{
    public class PermissionReturnDto
    {

            public int Id { get; set; }
            public string PermissionKey { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public List<UserDto> Users { get; set; } = new();
    }
}
