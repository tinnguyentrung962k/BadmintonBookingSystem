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
        public BadmintonCenterService(IBadmintonCenterRepository badmintonCenterRepository, IUnitOfWork unitOfWork)
        {
            _badmintonCenterRepository = badmintonCenterRepository;
            _unitOfWork = unitOfWork;

        }

        public async Task CreateBadmintonCenter(BadmintonCenterEntity badmintonCenterEntity)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var bcEntity = _badmintonCenterRepository.Add(badmintonCenterEntity);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<BadmintonCenterEntity>> GetAllBadmintonCenterAsync(int pageIndex, int size)
        {
            var badmintonCenter = await _badmintonCenterRepository.GetPagingAsync(pageIndex, size);
            if (!badmintonCenter.Any()) 
            {
                throw new NotFoundException("Empty list!");
            }
            return badmintonCenter;
        }

        public async Task<BadmintonCenterEntity> GetBadmintonCenterByIdAsync(string centerId)
        {
            var chosenCenter = await _badmintonCenterRepository.GetOneAsync(centerId);
            if (chosenCenter == null)
            {
                throw new NotFoundException("Not Found!");
            }
            return chosenCenter;
        }

        public virtual async Task<BadmintonCenterEntity> UpdateBadmintonInfo(BadmintonCenterEntity badmintonCenterEntity, string centerId)
        {
            var badmintonCenter = await GetBadmintonCenterByIdAsync(centerId) ;
            badmintonCenter.Name = badmintonCenterEntity.Name;
            badmintonCenter.Location = badmintonCenterEntity.Location;
            badmintonCenter.LastUpdatedTime = DateTimeOffset.UtcNow;
             _badmintonCenterRepository.Update(badmintonCenter);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();
            return badmintonCenter;
        }
    }
}
