using AutoMapper;
using BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs;
using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonBookingSystem.Controllers
{
    
    [ApiController]
    public class BadmintonCenterController : ControllerBase
    {
        private readonly IBadmintonCenterService _badmintonCenterService;
        private readonly IMapper _mapper;
        public BadmintonCenterController(IBadmintonCenterService badmintonCenterService, IMapper mapper)
        {
            _badmintonCenterService = badmintonCenterService;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("api/[controller]/badminton-centers")]
        public async Task<ActionResult<List<ResponseBadmintonCenterDTO>>> GetAllBadmintonCenters(int postion, int size) 
        {
            try 
            {
                var badmintonCenter = _mapper.Map<List<ResponseBadmintonCenterDTO>>(await _badmintonCenterService.GetAllBadmintonCenterAsync(postion, size));
                return Ok(badmintonCenter);
            }
            catch (NotFoundException ex) 
            {
                return NotFound(ex.Message);
            }
        }
    }
}
