﻿using AutoMapper;
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
        [HttpPost]
        [Route("api/badminton-centers")]
        public async Task<ActionResult<ResponseBadmintonCenterDTO>> CreateBadmintonCenter([FromForm]BadmintonCenterCreateDTO badmintonCenterCreateDTO)
        {
            try
            {
                var newBcEntity = _mapper.Map<BadmintonCenterEntity>(badmintonCenterCreateDTO);
                await _badmintonCenterService.CreateBadmintonCenter(newBcEntity,badmintonCenterCreateDTO.ImageFiles);
                var responseNewBc = _mapper.Map<ResponseBadmintonCenterDTO>(newBcEntity);
                return CreatedAtAction(nameof(GetBadmintonCenterById), new { id = responseNewBc.Id }, responseNewBc);
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
                var badmintonToUpdate = await _badmintonCenterService.UpdateBadmintonInfo(_mapper.Map<BadmintonCenterEntity>(badmintonUpdateDTO), id,badmintonUpdateDTO.ImageFiles);
                var updatedCenter = _mapper.Map<ResponseBadmintonCenterDTO>(badmintonToUpdate);
                return Ok(updatedCenter);
            }
            catch (Exception ex)
            {
                return BadRequest("Update Failed !");
            }
        }
    }
}
