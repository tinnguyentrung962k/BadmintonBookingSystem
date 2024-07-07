using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs
{
    public class CourtCreateDTO
    {
        [Required]
        public string CourtName { get; set; }
        [Required]
        public string CenterId { get; set; }
        public List<IFormFile>? ImageFiles { get; set; }
    }
}
