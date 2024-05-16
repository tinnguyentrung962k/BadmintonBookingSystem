using BadmintonBookingSystem.DataAccessLayer.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.DataAccessLayer.Entity
{
    [Table("User")]
    public class UserEntity : BaseEntity
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public Sex Sex { get; set; }
        [Required]
        public DateTime Dob { get; set; }
        [Required]
        public string RoleId { get; set; }

        [ForeignKey(nameof(RoleId))]
        public virtual RoleEntity Role { get; set; }

        public IEnumerable<BookingOrderEntity> BookingOrderEntities { get; set; }


    }
}
