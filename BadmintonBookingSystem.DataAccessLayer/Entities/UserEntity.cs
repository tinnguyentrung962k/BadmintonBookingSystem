using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.DataAccessLayer.Entities
{
    [Table("User")]
    public class UserEntity : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset LastUpdatedTime { get; set; } = DateTimeOffset.UtcNow;
        public string? RefreshToken { get; set; }
        [JsonIgnore] public virtual IEnumerable<UserRoleEntity> UserRoles { get; }
        [JsonIgnore] public virtual IEnumerable<BookingOrderEntity> BookingOrders { get; }
    }
}
