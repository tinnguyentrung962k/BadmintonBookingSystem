using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Service.Services.Interface
{
    public interface IUserService
    {
        Task<LoginResponseDTO> Login(string email, string password);

        Task Register(UserEntity user, string password);

        Task<string> Refresh(string refreshToken);

        Task Revoke(string userId);
        Task<UserEntity> GetUserByUserName(string name);

        Task<UserEntity> GetUserWithUserRolesById(string userId);

        Task<UserEntity> GetUserWithId(string id);

        Task<IEnumerable<UserEntity>> GetUsersList(int pageIndex, int pageSize);
        Task<IEnumerable<UserEntity>> GetUsersManager(int pageIndex, int pageSize);
        Task<UserEntity> UpdateUser(string userId, UpdateUserDTO updateUserDTO);
        Task<UserEntity> DeactiveUser(string userId);
        Task<IEnumerable<UserEntity>> SearchGetUsersList(int pageIndex, int pageSize, SearchUserDTO searchUserDTO);
        Task<UserEntity> AddNewUser(UserEntity newUser, string password, string roleName);


    }
}
