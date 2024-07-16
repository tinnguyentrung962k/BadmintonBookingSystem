using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs
{
    public class ResponseCourtDTO
    {
        public string Id { get; set; }
        public string CourtName { get; set; }
        public string CenterId { get; set; }
        public string CenterName { get; set; }
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "courtImgUrls")]
        [JsonPropertyName("courtImgUrls")]
        public IEnumerable<string> ImgUrls { get; set; }
    }
}
