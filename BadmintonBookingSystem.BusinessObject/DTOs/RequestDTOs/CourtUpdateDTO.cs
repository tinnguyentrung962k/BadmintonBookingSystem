using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs
{
    public class CourtUpdateDTO
    {
        [Required(ErrorMessage = "Please fill court name")]
        public string CourtName { get; set; }
        public List<IFormFile>? ImageFiles { get; set; }
    }
}
