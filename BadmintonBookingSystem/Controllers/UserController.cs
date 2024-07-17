using AutoMapper;
using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs;
using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Repository.Repositories.Extensions;
using BadmintonBookingSystem.Service.Services;
using BadmintonBookingSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace BadmintonBookingSystem.Controllers
{
    
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly RoleManager<RoleEntity> _roleManager;
        public UserController(IUserService userService, IMapper mapper, RoleManager<RoleEntity> roleManager)
        {
            _userService = userService;
            _mapper = mapper;
            _roleManager = roleManager;
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
        [Route("api/users/update/{userId}")]
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
        [Route("api/users/deactive/{userId}")]
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
        public async Task<ActionResult> SearchUser([FromQuery]int pageIndex, int pageSize, [FromQuery] SearchUserDTO searchUser)
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
    }
}
