using AutoMapper;
using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs;
using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Service.Services;
using BadmintonBookingSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BadmintonBookingSystem.Controllers
{
    [ApiController]
    public class CourtController : ControllerBase
    {
        private readonly ICourtService _courtService;
        private IMapper _mapper;
        public CourtController(ICourtService courtService, IMapper mapper)
        {
            _courtService = courtService;
            _mapper = mapper;
        }
        [HttpGet("api/[controller]/courts/{centerId}")]
        public async Task<ActionResult<List<ResponseCourtDTO>>> GetAllCourtsByCenterId([FromRoute] string centerId,[FromQuery] int pageIndex, int size)
        {
            try
            {
                var courtList = _mapper.Map<List<ResponseCourtDTO>>(await _courtService.GetAllCourtByCenterId(centerId,pageIndex, size));
                return Ok(courtList);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("api/[controller]/court/{id}")]
        public async Task<IActionResult> GetCourtById([FromRoute]string id)
        {
            try
            {
                var chosenCourt= _mapper.Map<ResponseCourtDTO>(await _courtService.GetCourtById(id));
                return Ok(chosenCourt);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server Error.");
            }
        }
        [HttpPost("api/[controller]/court")]
        public async Task<ActionResult<ResponseCourtDTO>> CreateCourt([FromForm] CourtCreateDTO courtCreateDTO)
        {
            try
            {
                var newCourtEntity = _mapper.Map<CourtEntity>(courtCreateDTO);
                await _courtService.CreateNewCourt(newCourtEntity,courtCreateDTO.ImageFiles);
                var responseNewCourt = _mapper.Map<ResponseCourtDTO>(newCourtEntity);
                return CreatedAtAction(nameof(GetCourtById), new { id = responseNewCourt.Id }, responseNewCourt);
            }
            catch (Exception ex)
            {
                return BadRequest("Created Failed !");
            }
        }
        [HttpPut("api/[controller]/court/{id}")]
        public async Task<IActionResult> EditCourt([FromForm] CourtUpdateDTO courtUpdateDTO, [FromRoute] string id)
        {
            try
            {
                var court = await _courtService.GetCourtById(id);
                var courtToUpdate = await _courtService.UpdateCourt(_mapper.Map<CourtEntity>(courtUpdateDTO), id, courtUpdateDTO.ImageFiles);
                var updatedCourt = _mapper.Map<ResponseCourtDTO>(courtToUpdate);
                return Ok(updatedCourt);
            }
            catch (Exception ex)
            {
                return BadRequest("Update Failed !");
            }
        }
    }
}
