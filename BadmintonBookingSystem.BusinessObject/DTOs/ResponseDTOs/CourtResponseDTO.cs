using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs
{
    public class CourtResponseDTO
    {
        public string CourtId { get; set; }
        public string CenterId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }

}
