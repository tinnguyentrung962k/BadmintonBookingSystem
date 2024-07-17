using AutoMapper;
using BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs;
using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.Service.Services;
using BadmintonBookingSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace BadmintonBookingSystem.Controllers
{
    
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
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
        [HttpPut]
        [Route("api/users/{userId}")]
        public async Task<ActionResult> UpdateUser(string userId, [FromBody] ResponseUpdateUserDTO updateUserDto)
        {
            try
            {
                await _userService.UpdateUser(userId, updateUserDto.FullName, updateUserDto.PhoneNumber);
                return Ok();
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
        [Route("api/users/Deactive/{userId}")]
        public async Task<ActionResult> DeactiveUser(string userId, bool status)
        {
            try
            {
                await _userService.DeactiveUser(userId, status);
                return Ok();
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
        [Route("api/users/Search")]
        public async Task<ActionResult> SearchUser(int pageSize, int pageIndex, string? name, string? email, string? phoneNumber)
        {
            try
            {
                var result = _mapper.Map<List<ResponseUserDTO>>(await _userService.SearchGetUsersList (pageSize, pageIndex, name, email, phoneNumber));
                return Ok(result);
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
    }
}
