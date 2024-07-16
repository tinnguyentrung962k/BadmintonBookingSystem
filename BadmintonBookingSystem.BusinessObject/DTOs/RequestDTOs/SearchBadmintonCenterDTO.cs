using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs
{
    public class SearchBadmintonCenterDTO
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
        public TimeOnly? OperatingTime { get; set; }
        public TimeOnly? ClosingTime { get; set; }
    }
}
