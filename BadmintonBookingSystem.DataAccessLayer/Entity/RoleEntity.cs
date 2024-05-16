using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.DataAccessLayer.Entity
{
    [Table("Role")]
    public class RoleEntity
    {
        [Key]
        public string RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }
        public IEnumerable<UserEntity> Users { get; set; }
    }
}
