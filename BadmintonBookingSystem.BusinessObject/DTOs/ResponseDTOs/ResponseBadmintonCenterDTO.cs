using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs
{
    public class ResponseBadmintonCenterDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string OperatingTime { get; set; }
        public string ClosingTime { get; set; }
        public string ManagerName { get; set; }
        public string ImgAvatar { get; set; }
        public bool IsActive { get; set; }

        [JsonProperty(PropertyName = "centerImgUrls")]
        [JsonPropertyName("centerImgUrls")]
        public IEnumerable<string>? ImgUrls { get; set; }
    }
}
