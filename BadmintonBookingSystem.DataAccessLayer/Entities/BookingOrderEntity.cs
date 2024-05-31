using BadmintonBookingSystem.DataAccessLayer.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.DataAccessLayer.Entities
{
    [Table("BookingOrder")]
    public class BookingOrderEntity : BaseAuditEntity<string>
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string CourtId { get; set; }
        [Required]
        public DateOnly BookingDate { get; set; }
        [Required]
        public TimeOnly BookingTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual UserEntity User { get; set; }
        [ForeignKey(nameof(CourtId))]
        public virtual CourtEntity Court { get; set; }
    }
}
