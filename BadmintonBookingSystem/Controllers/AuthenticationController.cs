using AutoMapper;
using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs;
using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BadmintonBookingSystem.Controllers
{
    [ApiController]
    [Route("api")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;

        public AccountController(IUserService accountService, ILogger<AccountController> logger, IMapper mapper)
        {
            _userService = accountService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                LoginResponseDTO res = await _userService.Login(loginDto.Email, loginDto.Password);
                return Ok(res);
            }
            catch (InvalidLoginException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while login");
                return StatusCode(500, "Lỗi đăng nhập xảy ra");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            try
            {
                await _userService.Register(_mapper.Map<UserEntity>(registerDto), registerDto.Password);
                return Ok("Tạo tài khoản thành công");
            }
            catch (ExistedEmailException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while register");
                return StatusCode(500, "Lỗi tạo tài khoản");
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenDTO refreshTokenDto)
        {
            try
            {
                var res = await _userService.Refresh(refreshTokenDto.RefreshToken);
                refreshTokenDto.AccessToken = res;
                return Ok(refreshTokenDto);
            }
            catch (NotFoundException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Server Error");
                return StatusCode(500, "Lỗi server");
            }

        }

        [Authorize]
        [HttpDelete("revoke")]
        public async Task<IActionResult> Revoke()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (userId == null)
                {
                    return Unauthorized();
                }
                await _userService.Revoke(userId);
                return Ok("Revoke Successfully !");
            }

            catch (Exception ex)
            {
                return BadRequest("Invalid refresh token");
            }


        }

    }
}
