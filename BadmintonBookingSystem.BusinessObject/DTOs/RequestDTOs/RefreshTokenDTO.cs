﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs
{
    public class RefreshTokenDTO
    {
        public string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
