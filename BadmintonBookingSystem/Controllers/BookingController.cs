﻿using AutoMapper;
using BadmintonBookingSystem.BusinessObject.Constants;
using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs;
using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace BadmintonBookingSystem.Controllers
{
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;
        public BookingController(IBookingService bookingService, IMapper mapper)
        {
            _bookingService = bookingService;
            _mapper = mapper;
        }

        [HttpPost("api/bookings/single-booking")]
        [Authorize(Roles = RoleConstants.CUSTOMER)]
        public async Task<ActionResult<ResponseBookingHeaderAndBookingDetail>> CreateBookingSingle(SingleBookingCreateDTO singleBookingCreateDTO)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var booking = _mapper.Map<BookingEntity>(await _bookingService.CreateBookingInSingleDay(userId, singleBookingCreateDTO));
                var bookingResponse = _mapper.Map<ResponseBookingHeaderAndBookingDetail>(booking);
                return StatusCode(201, bookingResponse);

            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ConflictException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server Error");
            }
        }
        [HttpPost("api/bookings/flex-booking")]
        [Authorize(Roles = RoleConstants.CUSTOMER)]
        public async Task<ActionResult<ResponseBookingHeaderAndBookingDetail>> CreateBookingFlex(List<FlexBookingCreateDTO> flexBookingCreateDTOs)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var booking = _mapper.Map<BookingEntity>(await _bookingService.CreateBookingFlex(userId, flexBookingCreateDTOs));
                var bookingResponse = _mapper.Map<ResponseBookingHeaderAndBookingDetail>(booking);
                return StatusCode(201, bookingResponse);

            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ConflictException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server Error");
            }
        }
        [HttpPost("api/bookings/fixed-booking")]
        [Authorize(Roles = RoleConstants.CUSTOMER)]
        public async Task<ActionResult<ResponseBookingHeaderAndBookingDetail>> CreateBookingFixed(FixedBookingCreateDTO fixedBookingCreateDTO)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var booking = _mapper.Map<BookingEntity>(await _bookingService.CreateBookingFixed(userId, fixedBookingCreateDTO));
                var bookingResponse = _mapper.Map<ResponseBookingHeaderAndBookingDetail>(booking);
                return StatusCode(201, bookingResponse);

            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ConflictException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server Error");
            }


        }

        [HttpGet("api/bookings/court-reservation/center/{id}")]
        public async Task<ActionResult<List<ResponseCourtReservationDTO>>> GetAllReservationsOfCenter([FromRoute] string id, [FromQuery] int pageIndex, int pageSize)
        {
            try
            {
                var bookingReservation = _mapper.Map<List<ResponseCourtReservationDTO>>(await _bookingService.GetAllBookingOfCustomersByCenterId(id, pageIndex, pageSize));
                return Ok(bookingReservation);
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

        [HttpGet("api/bookings/search-court-reservation/center/{id}")]
        public async Task<ActionResult<List<ResponseCourtReservationDTO>>> SearchReservationsOfCenter([FromRoute] string id, [FromQuery]SearchBookingDTO searchBookingDTO, [FromQuery] int pageIndex, int pageSize)
        {
            try
            {
                var bookingReservation = _mapper.Map<List<ResponseCourtReservationDTO>>(await _bookingService.SearchBookingOfCustomerByCenterId(id, searchBookingDTO, pageIndex, pageSize));
                return Ok(bookingReservation);
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
