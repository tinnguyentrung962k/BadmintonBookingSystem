using AutoMapper;
using BadmintonBookingSystem.BusinessObject.Constants;
using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs;
using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Service.Services;
using BadmintonBookingSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("api/courts/center/{centerId}")]
        [Authorize(Roles = RoleConstants.MANAGER)]
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
            catch (Exception ex)
            {
                return StatusCode(500, "Server Error.");
            }
        }
        [HttpGet("api/courts-active/center/{centerId}")]
        [Authorize(Roles = RoleConstants.CUSTOMER)]
        public async Task<ActionResult<List<ResponseCourtDTO>>> GetAllActiveCourtsByCenterId([FromRoute] string centerId, [FromQuery] int pageIndex, int size)
        {
            try
            {
                var courtList = _mapper.Map<List<ResponseCourtDTO>>(await _courtService.GetAllActiveCourtsByCenterId(centerId, pageIndex, size));
                return Ok(courtList);
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

        [HttpGet("api/courts/{id}")]
        public async Task<ActionResult<ResponseCourtDTO>> GetCourtById([FromRoute]string id)
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
        [HttpPost("api/courts")]
        [Authorize(Roles = RoleConstants.MANAGER)]
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
        [HttpPut("api/courts/{id}")]
        [Authorize(Roles = RoleConstants.MANAGER)]
        public async Task<ActionResult<ResponseCourtDTO>> EditCourt([FromForm] CourtUpdateDTO courtUpdateDTO, [FromRoute] string id)
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

        [HttpPut("api/courts-toggle/{id}")]
        [Authorize(Roles = RoleConstants.MANAGER)]
        public async Task<ActionResult<ResponseCourtDTO>> ToggleStatusCourt([FromRoute] string id)
        {
            try
            {
                await _courtService.ToggleStatusCourt(id);
                var deactCourt = await _courtService.GetCourtById(id);
                var deactCourtResponse = _mapper.Map<ResponseCourtDTO>(deactCourt);
                return Ok(deactCourtResponse);
            }
            catch (Exception ex)
            {
                return BadRequest("Update Failed !");
            }
        }
    }
}
