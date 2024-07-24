using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs
{
    public class ResponseTimeSlotDTO
    {
        public string Id { get; set; }
        public string CourtId { get; set; }
        public string CourtName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

    }
}
