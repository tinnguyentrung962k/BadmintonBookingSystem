using AutoMapper;
using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
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
    [AllowAnonymous]
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
        [Route("api/users/Update/{userId}")]
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
        public async Task<ActionResult> DeactiveUser(string userId )
        {
            try
            {
                await _userService.DeactiveUser(userId);
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
        [HttpGet]
        [Route("api/users/Search")]
        public async Task<ActionResult> SearchUser([FromQuery]int pageSize, [FromQuery] int pageIndex, [FromQuery] SearchUserDTO searchUser)
        {
            try
            {
                if (searchUser == null)
                {
                    return BadRequest("Search criteria is required.");
                }
                var users = await _userService.SearchGetUsersList(pageSize, pageIndex, searchUser.FullName, searchUser.Email, searchUser.PhoneNumber);
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
    }
}
