﻿using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Service.Services.Interface
{
    public interface IBadmintonCenterService
    {
        Task<IEnumerable<BadmintonCenterEntity>> GetAllBadmintonCenterAsync(int pageIndex, int size);
        Task CreateBadmintonCenter(BadmintonCenterEntity badmintonCenterEntity, List<IFormFile>? picList, IFormFile avatar);
        Task<BadmintonCenterEntity> GetBadmintonCenterByIdAsync(string centerId);
        Task<IEnumerable<BadmintonCenterEntity>> SearchBadmintonCentersAsync(BadmintonCenterEntity searchBadmintonCenter, int pageIndex, int pageSize);
        Task<BadmintonCenterEntity> UpdateBadmintonInfo(BadmintonCenterEntity badmintonCenterEntity, string centerId, List<IFormFile>? newPicList, IFormFile? newAvatar);
        Task ToggleStatusBadmintonCenter(string centerId);
        Task<IEnumerable<BadmintonCenterEntity>> GetAllActiveBadmintonCentersAsync(int pageIndex, int size);
        Task<IEnumerable<BadmintonCenterEntity>> GetAllBadmintonCenterByManagerIdAsync(string managerId);
    }
}
