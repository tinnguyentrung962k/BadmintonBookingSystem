using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Repository.Repositories.Interface;
using BadmintonBookingSystem.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Service.Services
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly ITimeSlotRepository _timeSlotRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICourtRepository _courtRepository;
        public TimeSlotService(ITimeSlotRepository timeSlotRepository, IUnitOfWork unitOfWork,ICourtRepository courtRepository)
        {
            _unitOfWork = unitOfWork;
            _timeSlotRepository = timeSlotRepository;
            _courtRepository = courtRepository;
        }
        public async Task<TimeSlotEntity> CreateATimeSlot(TimeSlotEntity timeSlotEntity)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var court = await _courtRepository.GetOneAsync(timeSlotEntity.CourtId);
                if (court is null)
                {
                    throw new NotFoundException("Court is not found!");
                }
                timeSlotEntity.Court = court;
                _timeSlotRepository.Add(timeSlotEntity);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return timeSlotEntity;
            }
            catch (Exception ex) 
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("Failed to create time slot", ex);
            }
        }

        public async Task<IEnumerable<TimeSlotEntity>> GetAllTimeSlotsByCourtId(string courtId, int pageIndex, int pageSize)
        {
            var timeSlotList = await _timeSlotRepository.QueryHelper()
                .Filter(ts => ts.CourtId.Equals(courtId))
                .Include(ts => ts.Court)
                .GetPagingAsync(pageIndex, pageSize);
            if (!timeSlotList.Any())
            {
                throw new NotFoundException("Empty List !");
            }
            return timeSlotList;
        }

        public async Task<TimeSlotEntity> GetTimeSlotById(string id)
        {
            var timeSlot = await _timeSlotRepository.QueryHelper()
                .Filter(ts => ts.Id.Equals(id))
                .Include(ts => ts.Court)
                .GetOneAsync();
            if (timeSlot is null)
            {
                throw new NotFoundException("Slot Not Found!");
            }
            return timeSlot;
        }
    }
}
