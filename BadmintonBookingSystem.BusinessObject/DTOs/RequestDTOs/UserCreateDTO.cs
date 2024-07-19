using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs
{
    public class UserCreateDTO : RegisterDTO
    {
        [Required(ErrorMessage = "Role is required")]
        public string RoleId { get; set; }
    }
}
