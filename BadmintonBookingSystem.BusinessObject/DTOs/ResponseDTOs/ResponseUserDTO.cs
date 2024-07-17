using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs
{
    public class ResponseUserDTO
    {
        public string Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        [JsonProperty(PropertyName = "authorities")]
        [JsonPropertyName("authorities")]
        public IEnumerable<string> UserRoles { get; set; }
        public bool IsActive { get; set; }
    }
}
