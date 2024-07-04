using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Repository.Repositories;
using BadmintonBookingSystem.Repository.Repositories.Interface;
using BadmintonBookingSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Service.Services
{
    public class BadmintonCenterService : IBadmintonCenterService
    {
        private readonly IBadmintonCenterRepository _badmintonCenterRepository;
        private readonly IUnitOfWork _unitOfWork;
        private UserManager<UserEntity> _userManager;
        public BadmintonCenterService(IBadmintonCenterRepository badmintonCenterRepository, IUnitOfWork unitOfWork, UserManager<UserEntity> userManager)
        {
            _badmintonCenterRepository = badmintonCenterRepository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task CreateBadmintonCenter(BadmintonCenterEntity badmintonCenterEntity)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                // Ensure the manager relationship is valid
                if (!string.IsNullOrEmpty(badmintonCenterEntity.ManagerId))
                {
                    // Check if the manager exists in the database
                    var manager = await _userManager.FindByIdAsync(badmintonCenterEntity.ManagerId);
                    if (manager == null)
                    {
                        throw new Exception("Manager not found"); // Handle appropriately
                    }
                }

                // Add the badminton center entity to the repository
                var bcEntity = _badmintonCenterRepository.Add(badmintonCenterEntity);

                // Save changes and commit transaction
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("Failed to create badminton center", ex);
            }
        }

        public async Task<IEnumerable<BadmintonCenterEntity>> GetAllBadmintonCenterAsync(int pageIndex, int size)
        {
            var badmintonCenter = await _badmintonCenterRepository.QueryHelper()
                .Include(x => x.Manager)
                .GetPagingAsync(pageIndex, size);
            if (!badmintonCenter.Any()) 
            {
                throw new NotFoundException("Empty list!");
            }
            return badmintonCenter;
        }

        public async Task<BadmintonCenterEntity> GetBadmintonCenterByIdAsync(string centerId)
        {
            var chosenCenter = await _badmintonCenterRepository
                .QueryHelper().
                Filter(c => c.Id == centerId)
                .Include(x=>x.Manager)
                .GetOneAsync();
            if (chosenCenter == null)
            {
                throw new NotFoundException("Not Found!");
            }
            return chosenCenter;
        }

        public async Task<BadmintonCenterEntity> UpdateBadmintonInfo(BadmintonCenterEntity badmintonCenterEntity, string centerId)
        {

            var badmintonCenter = await GetBadmintonCenterByIdAsync(centerId) ;
            if (!string.IsNullOrEmpty(badmintonCenterEntity.ManagerId))
            {
                // Check if the manager exists in the database
                var manager = await _userManager.FindByIdAsync(badmintonCenterEntity.ManagerId);
                if (manager == null)
                {
                    throw new Exception("Manager not found"); // Handle appropriately
                }
            }
            badmintonCenter.Name = badmintonCenterEntity.Name;
            badmintonCenter.Location = badmintonCenterEntity.Location;
            badmintonCenter.ManagerId = badmintonCenterEntity.ManagerId;
            badmintonCenter.OperatingTime = badmintonCenterEntity.OperatingTime;
            badmintonCenter.LastUpdatedTime = DateTimeOffset.UtcNow;
             _badmintonCenterRepository.Update(badmintonCenter);
            await _unitOfWork.SaveChangesAsync();
            return badmintonCenter;
        }
    }
}
