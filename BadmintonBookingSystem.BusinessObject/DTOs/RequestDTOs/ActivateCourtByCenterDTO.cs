using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs
{
    public class ActivateCourtByCenterDTO
    {
        public string CenterId { get; set; }
        public bool IsActive { get; set; }
    }

}
