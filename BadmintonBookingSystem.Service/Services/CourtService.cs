using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
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
    public class CourtService : ICourtService
    {
        private readonly ICourtRepository _courtRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBadmintonCenterRepository _badmintonCenterRepository;

        public CourtService(IUnitOfWork unitOfWork, ICourtRepository courtRepository,IBadmintonCenterRepository badmintonCenterRepository) 
        {
            _unitOfWork = unitOfWork;
            _courtRepository = courtRepository;
            _badmintonCenterRepository = badmintonCenterRepository;
        }
        public async Task CreateNewCourt(CourtEntity courtEntity)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var badmintonCenter = await _badmintonCenterRepository.GetOneAsync(courtEntity.CenterId);
                if (badmintonCenter == null)
                {
                    throw new NotFoundException("Chosen Center not found");
                }
                // Add the badminton center entity to the repository
                var bcEntity = _courtRepository.Add(courtEntity);
                // Save changes and commit transaction
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("Failed to create court", ex);
            }
        }

        public async Task<IEnumerable<CourtEntity>> GetAllCourtByCenterId(string centerId, int pageIndex, int size)
        {
            var chosenCenter = await _badmintonCenterRepository.GetOneAsync(centerId);
            if (chosenCenter == null)
            {
                throw new NotFoundException("Chosen Badminton Center is not found");
            }
            var courtList = await _courtRepository.QueryHelper().Include(c => c.BadmintonCenter)
                .Filter(c => c.CenterId.Equals(centerId))
                .GetPagingAsync(pageIndex, size);
            if (!courtList.Any())
            {
                throw new NotFoundException("Empty List !");
            }
            return courtList;
        }

        public async Task<CourtEntity> GetCourtById(string courtId)
        {
            var chosenCourt = await _courtRepository.QueryHelper()
                .Filter(c => c.Id == courtId)
                .Include(x => x.BadmintonCenter)
                .GetOneAsync();
            if (chosenCourt == null)
            {
                throw new NotFoundException("Not Found!");
            }
            return chosenCourt;
        }

        public Task UpdateCourt(CourtEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
