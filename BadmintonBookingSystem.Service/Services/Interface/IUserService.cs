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
        Task UpdateUser(string userId, string fullName, string phoneNumber);
        Task DeactiveUser(string userId, bool status);
        Task<IEnumerable<UserEntity>> SearchGetUsersList(int pageIndex, int pageSize, string name,string email, string phoneNumber);
    }
}
