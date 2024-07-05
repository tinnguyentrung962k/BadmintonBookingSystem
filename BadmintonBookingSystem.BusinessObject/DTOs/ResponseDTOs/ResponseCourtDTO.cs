using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs
{
    public class ResponseCourtDTO
    {
        public string Id { get; set; }
        public string CourtName { get; set; }
        public string CenterName { get; set; }
    }
}
