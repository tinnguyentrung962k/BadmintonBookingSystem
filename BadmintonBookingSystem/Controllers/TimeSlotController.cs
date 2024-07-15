using AutoMapper;
using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs;
using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonBookingSystem.Controllers
{
    [ApiController]
    public class TimeSlotController : ControllerBase
    {
        private readonly ITimeSlotService _timeSlotService;
        private readonly IMapper _mapper;

        public TimeSlotController(ITimeSlotService timeSlotService, IMapper mapper)
        {
            _mapper = mapper;
            _timeSlotService = timeSlotService;
        }

        [HttpPost("api/timeslots")]
        public async Task<ActionResult<ResponseTimeSlotDTO>> CreateTimeSlot(TimeSlotCreateDTO timeSlotCreateDTO)
        {
            try
            {
                var newTimeSlot = _mapper.Map<TimeSlotEntity>(timeSlotCreateDTO);
                var responseTimeSlot = _mapper.Map<ResponseTimeSlotDTO>(await _timeSlotService.CreateATimeSlot(newTimeSlot));
                return StatusCode(201, responseTimeSlot);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("api/timeslots/court/{courtId}")]
        public async Task<ActionResult<List<ResponseTimeSlotDTO>>> GetTimeSlotByCourtId([FromRoute] string courtId, [FromQuery] int pageIndex, int pageSize)
        {
            try
            {
                var responseTimeSlots = _mapper.Map<List<ResponseTimeSlotDTO>>(await _timeSlotService.GetAllTimeSlotsByCourtId(courtId, pageIndex, pageSize));
                return Ok(responseTimeSlots);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server Error");
            }
        }

        [HttpGet("api/timeslots/{id}")]
        public async Task<ActionResult<ResponseTimeSlotDTO>> GetTimeSlotById([FromRoute] string id)
        {
            try
            {
                var responseTimeSlot = _mapper.Map<ResponseTimeSlotDTO>(await _timeSlotService.GetTimeSlotById(id));
                return Ok(responseTimeSlot);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server Error");
            }

        }


    }
}
