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
    [Table("Booking")]
    public class BookingEntity : BaseAuditEntity<string>
    {
        public string CustomerId { get; set; }
        
        [ForeignKey(nameof(CustomerId))]
        public UserEntity Customer { get; set; }
        public string TimeSlotId { get; set; }

        [ForeignKey(nameof(TimeSlotId))]
        public TimeSlotEntity TimeSlot { get; set; }
        public DateOnly? BookingDate { get; set; }
        public BookingType BookingType { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public string? DayOfAWeek { get; set; }
    }
}
