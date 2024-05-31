using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.DataAccessLayer.Entities
{
    [Table("Role")]
    public class RoleEntity : IdentityRole<string>
    {
        public ICollection<UserRoleEntity> UserRoles { get; set; }
    }
}
