using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs
{
    public class ResponseBookingDetailDTO
    {
        public string Id { get; set; }
        public string BookingId { get; set; }
        public string TimeSlotId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public DateOnly BookingDate { get; set; }
        public string DayOfAWeek { get; set; }
        public decimal SlotPrice { get; set; }

    }
}
