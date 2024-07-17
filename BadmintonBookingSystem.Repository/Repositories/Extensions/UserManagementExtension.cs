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
        public static async Task<IEnumerable<UserEntity>> GetUsersAsyncInASpecificRole(this UserManager<UserEntity> userManager, string roleName, int pageIndex = 1, int pageSize = 1)
        {
            var userList = await userManager?.Users?
                .Include(it => it.UserRoles)
                .ThenInclude(r => r.Role)
                .Where(user => user.UserRoles.Any(userRole => userRole.Role.Name == roleName))
                .ToListAsync();
            pageIndex = pageIndex < 1 ? 0 : pageIndex - 1;
            pageSize = pageSize < 1 ? 10 : pageSize;
            var pagedUsers = userList.Skip(pageIndex * pageSize).Take(pageSize);
            return pagedUsers;
        }
        public static async Task<IEnumerable<UserEntity>> GetUsersWithRoleAsync(this UserManager<UserEntity> userManager, int pageIndex = 1, int pageSize = 1)
        {
            var userList = await userManager?.Users?
                .Include(it => it.UserRoles)
                .ThenInclude(r => r.Role)
                .ToListAsync();
            pageIndex = pageIndex < 1 ? 0 : pageIndex - 1;
            pageSize = pageSize < 1 ? 10 : pageSize;
            var pagedUsers = userList.Skip(pageIndex * pageSize).Take(pageSize);
            return pagedUsers;

        }

        public static async Task<IEnumerable<UserEntity>> GetUsersWithRoleWithoutPaginationAsync(this UserManager<UserEntity> userManager)
        {
            var userList = await userManager?.Users?
                .Include(it => it.UserRoles)
                .ThenInclude(r => r.Role)
                .ToListAsync();
            return userList;
        }

        public static async Task<IEnumerable<UserEntity>> GetPagingAsync(this UserManager<UserEntity> userManager, IEnumerable<UserEntity> userList, int pageIndex = 1, int pageSize = 1)
        {
            pageIndex = pageIndex < 1 ? 0 : pageIndex - 1;
            pageSize = pageSize < 1 ? 10 : pageSize;
            var pagedUsers = userList.Skip(pageIndex * pageSize).Take(pageSize);
            return pagedUsers;
        }

    }
}
