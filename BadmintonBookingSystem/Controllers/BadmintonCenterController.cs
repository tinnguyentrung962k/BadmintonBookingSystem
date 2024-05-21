using BadmintonBookingSystem.Service.Interface;
using BadmintonBookingSystem.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonBookingSystem.Controllers
{
    
    [ApiController]
    public class BadmintonCenterController : ControllerBase
    {
        private readonly IBadmintonCenterService _badmintonCenterService;
        public BadmintonCenterController(IBadmintonCenterService badmintonCenterService)
        {
            _badmintonCenterService = badmintonCenterService;
        }
        //[HttpGet]
        //[Route("/api/v1/club")]
        //public async Task<IActionResult> GetBadmintonCenters()
        //{
        //   var clubs = await _badmintonCenterService.GetAllBadmintonCenters();
        //   return Ok(clubs);
        //}
    }
}
