using BadmintonBookingSystem.BusinessObject.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs
{
    public class ResponseBookingHeaderDTO
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string BookingType { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }

    }
}
