﻿using AutoMapper;
using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs;
using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Repository.Repositories.Extensions;
using BadmintonBookingSystem.Service.Services;
using BadmintonBookingSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace BadmintonBookingSystem.Controllers
{

    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICourtService _courtService;
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;
        private readonly RoleManager<RoleEntity> _roleManager;
        public UserController(IUserService userService, ICourtService courtService, IBookingService bookingService, IMapper mapper, RoleManager<RoleEntity> roleManager)
        {
            _userService = userService;
            _mapper = mapper;
            _roleManager = roleManager;
            _courtService = courtService;
            _bookingService = bookingService;
        }

        [HttpGet]
        [Route("api/users")]
        public async Task<ActionResult<List<ResponseUserDTO>>> GetAllUsers(int pageIndex, int pageSize) 
        {
            try
            {
                var userList = _mapper.Map<List<ResponseUserDTO>>(await _userService.GetUsersList(pageIndex, pageSize));
                return Ok(userList);
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
        [Route("api/users/managers")]
        public async Task<ActionResult<List<ResponseUserDTO>>> GetAllUsersManager(int pageIndex, int pageSize)
        {
            try
            {
                var userList = _mapper.Map<List<ResponseUserDTO>>(await _userService.GetUsersManager(pageIndex, pageSize));
                return Ok(userList);
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
        [HttpPut]
        [Route("api/users/update/{userId}")]
        public async Task<ActionResult<ResponseUserDTO>> UpdateUser(string userId, [FromBody] UpdateUserDTO updateUserDto)
        {
            try
            {
                var responseUser = _mapper.Map<ResponseUserDTO>(await _userService.UpdateUser(userId, updateUserDto));
                return Ok(responseUser);
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
        [HttpPut]
        [Route("api/users/deactive/{userId}")]
        public async Task<ActionResult<ResponseUserDTO>> DeactiveUser(string userId )
        {
            try
            {
                var responseUser = _mapper.Map<ResponseUserDTO>(await _userService.DeactiveUser(userId));
                return Ok(responseUser);
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
        [Route("api/users/Search")]
        public async Task<ActionResult<List<ResponseUserDTO>>> SearchUser([FromQuery]int pageIndex, int pageSize, [FromQuery] SearchUserDTO searchUser)
        {
            try
            {
                if (searchUser == null)
                {
                    return BadRequest("Search criteria is required.");
                }
                var users = await _userService.SearchGetUsersList(pageIndex, pageSize, searchUser);
                var result = _mapper.Map<List<ResponseUserDTO>>(users);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpPost]
        [Route("api/users/add")]
        public async Task<IActionResult> AddNewUser([FromBody] UserCreateDTO userCreateDTO)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(userCreateDTO.RoleId);
                await _userService.AddNewUser(_mapper.Map<UserEntity>(userCreateDTO), userCreateDTO.Password, role.Name);
                return StatusCode(201,"Tạo tài khoản thành công");
            }
            catch (ExistedEmailException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi tạo tài khoản");
            }
        }
        [HttpGet]
        [Route("api/roles")]
        public async Task<ActionResult<List<RoleResponseDTO>>> GetAllRoles()
        {
            try
            {
                var responseRoleList = _mapper.Map<List<RoleResponseDTO>>(await _roleManager.GetAllUserRolesAsync());
                return Ok(responseRoleList);
            }
            catch
            {
                return StatusCode(500, "Server Error");
            }
        }
        [HttpPost("activate-court")]
        public async Task<ActionResult<List<CourtResponseDTO>>> ActivateCourtByCenter([FromBody] ActivateCourtByCenterDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.CenterId))
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var result = await _courtService.ActivateCourtByCenterIdAsync(dto.CenterId, dto.IsActive);

                if (result)
                {
                    var updatedCourts = await _courtService.GetCourtsByCenterIdAsync(dto.CenterId);
                    var response = updatedCourts.Select(c => new CourtResponseDTO
                    {
                        CenterId = c.CenterId,
                        Name = c.CourtName,
                        IsActive = c.IsActive
                    }).ToList();

                    return Ok(response);
                }
                else
                {
                    return NotFound("No courts found for the given center ID.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("user/{userId}/details")]
        public async Task<ActionResult<List<ResponseBookingHeaderAndBookingDetail>>> GetBookingDetailsByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid user ID.");
            }

            try
            {
                var bookingDetails = await _bookingService.GetBookingDetailsByUserIdAsync(userId);
                if (bookingDetails == null || !bookingDetails.Any())
                {
                    return NotFound("No bookings found for this user.");
                }

                return Ok(bookingDetails);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log exception (optional: use a logging framework like Serilog, NLog, etc.)
                return StatusCode(500, "Internal server error.");
            }
        }   
    }
}
