﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs
{
    public class UpdateUserDTO
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string RoleId { get; set; }
    }
}
