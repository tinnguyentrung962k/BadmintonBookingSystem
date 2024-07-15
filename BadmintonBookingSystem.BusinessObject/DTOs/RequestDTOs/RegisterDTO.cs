using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs
{
    public class RegisterDTO
    {
        [EmailAddress]
        [Required(ErrorMessage = "Please fill your email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please fill your password")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters and less than 40 characters long.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Please fill your confirmed password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirmed password doesn't match")]
        public string? ConfirmedPassword { get; set; }

        [Required(ErrorMessage = "Please fill your full name")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Please fill your phone number")]
        public string? PhoneNumber { get; set; }

    }
}
