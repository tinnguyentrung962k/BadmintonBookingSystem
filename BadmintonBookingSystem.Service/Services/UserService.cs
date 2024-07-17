using BadmintonBookingSystem.BusinessObject.Constants;
using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs;
using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Repository.Repositories.Extensions;
using BadmintonBookingSystem.Repository.Repositories.Interface;
using BadmintonBookingSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Service.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IEmailService _emailService;
        private readonly ILogger<UserService> _logger;
        private readonly RoleManager<RoleEntity> _roleManager;
        public UserService(ITokenService tokenService,
            UserManager<UserEntity> userManager, IUnitOfWork unitOfWork,
            IConfiguration configuration,
            ILogger<UserService> logger, 
            IHttpContextAccessor httpContextAccessor, 
            RoleManager<RoleEntity> roleManager
            )
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _roleManager = roleManager;
        }

        public async Task<LoginResponseDTO> Login(string email, string password)
        {
            var loginUser = await _userManager.FindByEmailAsync(email);
            if (loginUser == null || !await _userManager.CheckPasswordAsync(loginUser, password))
            {
                throw new InvalidLoginException("Đăng nhập thất bại!");
            }
            var user = await _userManager.FindByIdAsync(loginUser.Id);
            string jwtToken;
            string refreshToken;
            if (!user.RefreshToken.IsNullOrEmpty())
            {
                jwtToken = _tokenService.CreateTokenForAccount(loginUser);
                return new LoginResponseDTO()
                {
                    AccessToken = jwtToken,
                    RefreshToken = user.RefreshToken
                };
            }
            else
            {
                jwtToken = _tokenService.CreateTokenForAccount(loginUser);
                refreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                await _userManager.UpdateAsync(user);
            }
            return new LoginResponseDTO
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken,
            };
        }


        public async Task Register(UserEntity registerUser, string password)
        {
            var existUser = await _userManager.FindByEmailAsync(registerUser.Email);
            if (existUser != null)
            {
                throw new ExistedEmailException("Email này đã tồn tại.");
            }

            try
            {
                _unitOfWork.BeginTransaction();
                registerUser.UserName = registerUser.Email;
                registerUser.EmailConfirmed = true;
                var result = await _userManager.CreateAsync(registerUser, password);
                if (!result.Succeeded)
                {
                    throw new InvalidRegisterException("Đăng ký thất bại");
                }
                await _userManager.AddToRoleAsync(registerUser, RoleConstants.CUSTOMER);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while register");
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<string> Refresh(string refreshToken)
        {
            var user = await _userManager.FindRefreshTokenAsync(refreshToken);
            if (user == null || !refreshToken.Equals(user.RefreshToken))
            {
                throw new NotFoundException("Không tìm thấy người dùng!");
            }
            var jwtToken = _tokenService.CreateTokenForAccount(user);
            return jwtToken;

        }

        public async Task Revoke(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("Không tìm thấy người dùng!");
            }
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
        }
        public async Task<UserEntity> GetUserByUserName(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            return user;
        }

        public async Task<UserEntity> GetUserWithUserRolesById(string userId)
        {
            return await _userManager.Users
                .Include(it => it.UserRoles)
                .ThenInclude(r => r.Role)
                .SingleOrDefaultAsync(it => it.Id == userId);
        }

        public async Task<UserEntity> GetUserWithId(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }

        public async Task<IEnumerable<UserEntity>> GetUsersList(int pageIndex, int pageSize)
        {
            var userList = await _userManager.GetUsersWithRoleAsync(pageIndex, pageSize);
            if (!userList.Any())
            {
                throw new NotFoundException("Không có người dùng nào trong danh sách");
            }
            return userList;
        }
        public async Task UpdateUser(string userId, string fullName, string phoneNumber) 
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("Không tìm thấy người dùng!");
            }

            user.FullName = fullName;
            user.PhoneNumber = phoneNumber;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception("Cập nhật người dùng thất bại.");
            }
        }

        public async Task DeactiveUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("Không tìm thấy người dùng!");
            }
            var status = user.EmailConfirmed;
            if (status == true)
            {
                user.EmailConfirmed = false;
            }
            else
            {
                user.EmailConfirmed = true;
            }
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception("Cập nhật người dùng thất bại.");
            }
        }

        public async Task<IEnumerable<UserEntity>> SearchGetUsersList(int pageIndex, int pageSize, SearchUserDTO searchUserDTO)
        {
            var allUser = await _userManager.GetUsersWithRoleWithoutPaginationAsync();
            if (!string.IsNullOrEmpty(searchUserDTO.FullName))
            {
                allUser = allUser
                    .Where(s => s.FullName.Contains(searchUserDTO.FullName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(searchUserDTO.Email))
            {
                allUser = allUser
                    .Where(s => s.Email.Contains(searchUserDTO.Email, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(searchUserDTO.PhoneNumber))
            {
                allUser = allUser
                    .Where(s => !string.IsNullOrEmpty(s.PhoneNumber) && s.PhoneNumber.Contains(searchUserDTO.PhoneNumber, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var pagedUsers = await _userManager.GetPagingAsync(allUser,pageIndex,pageSize);
            return pagedUsers;
        }

        public async Task<UserEntity> AddNewUser(UserEntity newUser, string password, string roleName)
        {
            var existUser = await _userManager.FindByEmailAsync(newUser.Email);
            if (existUser != null)
            {
                throw new ExistedEmailException("Email này đã tồn tại.");
            }

            try
            {
                _unitOfWork.BeginTransaction();
                newUser.UserName = newUser.Email;
                newUser.EmailConfirmed = true;
                var result = await _userManager.CreateAsync(newUser, password);
                if (!result.Succeeded)
                {
                    throw new InvalidRegisterException("Thêm thất bại!");
                }
                await _userManager.AddToRoleAsync(newUser, roleName);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return newUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while register");
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
