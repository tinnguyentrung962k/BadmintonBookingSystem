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
    [Table("TimeSlot")]
    public class TimeSlotEntity : BaseAuditEntity<string>
    {
        public string CourtId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public BookingType BookingType { get; set; }
        public decimal Price { get; set; }

        [ForeignKey(nameof(CourtId))]
        public CourtEntity Court { get; set; }


    }
}
