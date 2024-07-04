using BadmintonBookingSystem.BusinessObject.Enum;
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
        public string CustomerId { get; set; }
        public string CourtId { get; set; }
        public DateOnly BookingDate { get; set; }
        public string TimeSlotId { get; set; }
        public bool IsCheckIn { get; set; } = false;
        public bool IsPaid { get; set; } = false;
        public BookingType BookingType { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual UserEntity Customer { get; set; }
        [ForeignKey(nameof(CourtId))]
        public virtual CourtEntity Court { get; set; }
        [ForeignKey(nameof(TimeSlotId))]
        public virtual TimeSlotEntity TimeSlot { get;set; }
    }
}
