using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BadmintonBookingSystem.Controllers
{
    internal class ResponseModel<T> : ModelStateDictionary
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}