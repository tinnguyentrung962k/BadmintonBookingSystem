using BadmintonBookingSystem.BusinessObject.Constants;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Repository.Repositories.Extensions
{
    public static class UserManagementExtensions
    {
        public static async Task<UserEntity>? FindRefreshTokenAsync(this UserManager<UserEntity> userManager, string refreshToken)
        {
            return await userManager?.Users?.FirstOrDefaultAsync(u => u.RefreshToken.Equals(refreshToken));
        }
        public static async Task<IEnumerable<UserEntity>> GetUsersAsync(this UserManager<UserEntity> userManager, int pageIndex = 1, int pageSize = 1, string roleName = RoleConstants.USER)
        {
            var userList = await userManager?.GetUsersInRoleAsync(roleName);
            pageIndex = pageIndex < 1 ? 0 : pageIndex - 1;
            pageSize = pageSize < 1 ? 10 : pageSize;
            var pagedUsers = userList.Skip(pageIndex * pageSize).Take(pageSize);
            return pagedUsers;
        }
    }
}
