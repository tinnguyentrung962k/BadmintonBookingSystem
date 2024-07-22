using BadmintonBookingSystem.BusinessObject.Constants;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Repository.Repositories.Extensions
{
    public static class RoleManagementExtension
    {
        public static async Task<IEnumerable<RoleEntity>> GetAllUserRolesAsync(this RoleManager<RoleEntity> roleManager)
        {
            var roleList = await roleManager.Roles.ToListAsync();
            return roleList;
        }
    }
}
