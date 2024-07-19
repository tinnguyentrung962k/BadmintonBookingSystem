using BadmintonBookingSystem.BusinessObject.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs
{
    public class FixedBookingCreateDTO
    {
        public List<string> ListTimeSlotId { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public DayOfAWeek DayOfAWeek { get; set; }

    }
}
