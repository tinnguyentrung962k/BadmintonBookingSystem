using BadmintonBookingSystem.BusinessObject.Enum;
using BadmintonBookingSystem.DataAccessLayer.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.DataAccessLayer.Entities
{
    [Table("BookingDetail")]
    public class BookingDetailEntity : BaseAuditEntity<string>
    {
        public string TimeSlotId { get; set; }

        [ForeignKey(nameof(TimeSlotId))]
        public TimeSlotEntity TimeSlot { get; set; }
        public string BookingId { get; set; }
        
        [ForeignKey(nameof(BookingId))]
        public BookingEntity Booking { get; set;}

        public DateOnly BookingDate { get; set; }
        public DayOfAWeek DayOfAWeek { get; set; }
    }
}
