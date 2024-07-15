using BadmintonBookingSystem.BusinessObject.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs
{
    public class TimeSlotCreateDTO
    {
        [Required(ErrorMessage = "Court is required")]
        public string CourtId { get; set; }

        [Required(ErrorMessage = "StartTime is required")]
        public TimeOnly StartTime { get; set; }

        [Required(ErrorMessage = "EndTime is required")]
        public TimeOnly EndTime { get; set; }

        [Required(ErrorMessage = "DayOfWeek is required")]
        public DayOfAWeek DayOfAWeek { get; set; }
    }
}
