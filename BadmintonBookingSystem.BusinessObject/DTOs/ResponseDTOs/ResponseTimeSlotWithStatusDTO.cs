using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs
{
    public class ResponseTimeSlotWithStatusDTO : ResponseTimeSlotDTO
    {
        public bool isBooked { get; set; }
    }
}
