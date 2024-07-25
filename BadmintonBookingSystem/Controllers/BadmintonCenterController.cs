using AutoMapper;
using BadmintonBookingSystem.BusinessObject.Constants;
using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs;
using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

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
        [Route("api/badminton-centers")]
        public async Task<ActionResult<List<ResponseBadmintonCenterDTO>>> GetAllBadmintonCenters([FromQuery]int pageIndex, int size) 
        {
            try 
            {
                var badmintonCenter = _mapper.Map<List<ResponseBadmintonCenterDTO>>(await _badmintonCenterService.GetAllBadmintonCenterAsync(pageIndex, size));
                return Ok(badmintonCenter);
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
        [HttpGet]
        [Authorize(Roles = RoleConstants.MANAGER)]
        [Route("api/badminton-centers/manager")]
        public async Task<ActionResult<List<ResponseBadmintonCenterDTO>>> GetAllBadmintonCentersByManager()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var badmintonCenter = _mapper.Map<List<ResponseBadmintonCenterDTO>>(await _badmintonCenterService.GetAllBadmintonCenterByManagerIdAsync(userId));
                return Ok(badmintonCenter);
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
        [HttpGet]
        [Route("api/badminton-centers-active")]
        public async Task<ActionResult<List<ResponseBadmintonCenterDTO>>> GetAllActiveBadmintonCenters([FromQuery] int pageIndex, int size)
        {
            try
            {
                var badmintonCenter = _mapper.Map<List<ResponseBadmintonCenterDTO>>(await _badmintonCenterService.GetAllActiveBadmintonCentersAsync(pageIndex, size));
                return Ok(badmintonCenter);
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

        [HttpGet]
        [Route("api/search-badminton-centers")]
        public async Task<ActionResult<List<ResponseSearchBadmintonCenterDTO>>> Search([FromQuery] SearchBadmintonCenterDTO searchBadmintonCenterDTO, [FromQuery] int pageIndex, int pageSize)
        {
            try
            {
                var newBcEntity = _mapper.Map<BadmintonCenterEntity>(searchBadmintonCenterDTO);
                var searchResult = await _badmintonCenterService.SearchBadmintonCentersAsync(newBcEntity,pageIndex,pageSize);
                var responseNewBc = _mapper.Map<List<ResponseSearchBadmintonCenterDTO>>(searchResult);
                return Ok(responseNewBc);
            }
            catch (NotFoundException ex)
            {
                return BadRequest("Something wrong");
            }
        }

        [HttpPost]
        [Route("api/badminton-centers")]
        public async Task<ActionResult<ResponseBadmintonCenterDTO>> CreateBadmintonCenter([FromForm]BadmintonCenterCreateDTO badmintonCenterCreateDTO)
        {
            try
            {
                var newBcEntity = _mapper.Map<BadmintonCenterEntity>(badmintonCenterCreateDTO);
                await _badmintonCenterService.CreateBadmintonCenter(newBcEntity,badmintonCenterCreateDTO.ImageFiles,badmintonCenterCreateDTO.ImgAvatar);
                var responseNewBc = _mapper.Map<ResponseBadmintonCenterDTO>(newBcEntity);
                return CreatedAtAction(nameof(GetBadmintonCenterById), new { id = responseNewBc.Id }, responseNewBc);
            }
            catch(ConflictException e) {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Created Failed !");
            }
        }
        [HttpGet("api/badminton-centers/{id}")]
        public async Task<ActionResult<ResponseBadmintonCenterDTO>> GetBadmintonCenterById([FromRoute] string id)
        {
            try
            {
                var chosenBadmintonCenter = _mapper.Map<ResponseBadmintonCenterDTO>(await _badmintonCenterService.GetBadmintonCenterByIdAsync(id));
                return Ok(chosenBadmintonCenter);
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
        [HttpPut("api/badminton-centers/{id}")]
        public async Task<ActionResult<ResponseBadmintonCenterDTO>> EditBadmintonCenter([FromForm] BadmintonUpdateDTO badmintonUpdateDTO, [FromRoute] string id)
        {
            try
            {
                var badmintonCenter = await _badmintonCenterService.GetBadmintonCenterByIdAsync(id);
                var badmintonToUpdate = await _badmintonCenterService.UpdateBadmintonInfo(_mapper.Map<BadmintonCenterEntity>(badmintonUpdateDTO), id, badmintonUpdateDTO.ImageFiles, badmintonUpdateDTO.ImgAvatar);
                var updatedCenter = _mapper.Map<ResponseBadmintonCenterDTO>(badmintonToUpdate);
                return Ok(updatedCenter);
            }
            catch (ConflictException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Update Failed !");
            }
        }
        [HttpPut("api/badminton-centers-toggle/{id}")]
        public async Task<ActionResult<ResponseBadmintonCenterDTO>> ToggleStatusCenter([FromRoute] string id)
        {
            try
            {
                await _badmintonCenterService.ToggleStatusBadmintonCenter(id);
                var deactCenter = await _badmintonCenterService.GetBadmintonCenterByIdAsync(id);
                var deactCenterResponse = _mapper.Map<ResponseBadmintonCenterDTO>(deactCenter);
                return Ok(deactCenterResponse);
            }
            catch (Exception ex)
            {
                return BadRequest("Update Failed !");
            }
        }
    }
}
