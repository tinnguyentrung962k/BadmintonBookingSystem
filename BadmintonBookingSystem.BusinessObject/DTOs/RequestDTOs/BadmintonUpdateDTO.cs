﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs
{
    public class BadmintonUpdateDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "OperatingTime is required.")]
        public TimeOnly OperatingTime { get; set; }

        [Required(ErrorMessage = "ClosingTime is required.")]
        public TimeOnly ClosingTime { get; set; }

        [Required(ErrorMessage = "ManagerId is required.")]
        public string ManagerId { get; set; }
        public List<IFormFile>? ImageFiles { get; set; }
        public IFormFile? ImgAvatar { get; set; }
    }
}
