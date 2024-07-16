using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs
{
    public class ResponseSearchBadmintonCenterDTO
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string OperatingTime { get; set; }
        public string ClosingTime { get; set; }
    }
}
