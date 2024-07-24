using AutoMapper;
using BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs;
using BadmintonBookingSystem.BusinessObject.Enum;
using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Repository.Repositories.Interface;
using BadmintonBookingSystem.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private readonly IMapper _mapper;
        public TimeSlotService(ITimeSlotRepository timeSlotRepository, IUnitOfWork unitOfWork,ICourtRepository courtRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _timeSlotRepository = timeSlotRepository;
            _courtRepository = courtRepository;
            _mapper = mapper;
        }
        public async Task<TimeSlotEntity> CreateATimeSlot(TimeSlotEntity timeSlotEntity)
        {
            var overlappingTimeSlots = await _timeSlotRepository.QueryHelper()
            .Filter(ts => ts.CourtId == timeSlotEntity.CourtId &&
                          ((ts.StartTime <= timeSlotEntity.StartTime && ts.EndTime > timeSlotEntity.StartTime) ||
                           (ts.StartTime < timeSlotEntity.EndTime && ts.EndTime >= timeSlotEntity.EndTime) ||
                           (ts.StartTime >= timeSlotEntity.StartTime && ts.EndTime <= timeSlotEntity.EndTime)))
            .GetAllAsync();

            if (overlappingTimeSlots.Any())
            {
                throw new ValidationException("The time slot overlaps with an existing time slot.");
            }
            try
            {
                _unitOfWork.BeginTransaction();
                var court = await _courtRepository.GetOneAsync(timeSlotEntity.CourtId);
                if (court is null)
                {
                    throw new NotFoundException("Court is not found!");
                }
                timeSlotEntity.Court = court;
                timeSlotEntity.IsActive = true;
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

        public Task<IEnumerable<TimeSlotEntity>> GetAllActiveTimeSlotsByCourtId(string courtId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TimeSlotEntity>> GetAllTimeSlotsByCourtId(string courtId)
        {
            var timeSlotList = await _timeSlotRepository.QueryHelper()
                .Filter(ts => ts.CourtId.Equals(courtId))
                .Include(ts => ts.Court)
                .GetAllAsync();
            if (!timeSlotList.Any())
            {
                throw new NotFoundException("Empty List !");
            }
            return timeSlotList;
        }

        public async Task<List<ResponseTimeSlotWithStatusDTO>> GetAvalableAndNotAvailableTimeSlotsByCourtId(string courtId, DateOnly chosenDate)
        {
            var court = await _courtRepository.QueryHelper()
                .Include(c => c.TimeSlots)
                .Filter(c => c.Id.Equals(courtId))
                .GetOneAsync();

            var availableTimeSlots = await _timeSlotRepository.QueryHelper()
                .Filter(ts => ts.CourtId.Equals(courtId) && !ts.BookingDetails.Any(bd => bd.BookingDate == chosenDate && (bd.ReservationStatus == ReservationStatus.NotCheckedIn || bd.ReservationStatus == ReservationStatus.Completed || bd.ReservationStatus == ReservationStatus.CheckedIn)) && ts.IsActive == true)
                .Include(ts => ts.Court)
                .GetAllAsync();

            var bookedTimeSlots = await _timeSlotRepository.QueryHelper()
                .Filter(ts => ts.CourtId.Equals(courtId) && ts.BookingDetails.Any(bd => bd.BookingDate == chosenDate && (bd.ReservationStatus == ReservationStatus.NotCheckedIn || bd.ReservationStatus == ReservationStatus.Completed || bd.ReservationStatus == ReservationStatus.CheckedIn)) && ts.IsActive == true)
                .Include(ts => ts.Court)
                .GetAllAsync();

            var availableTimeSlotDtos = _mapper.Map<IEnumerable<ResponseTimeSlotWithStatusDTO>>(availableTimeSlots);
            var bookedTimeSlotDtos = _mapper.Map<IEnumerable<ResponseTimeSlotWithStatusDTO>>(bookedTimeSlots);

            foreach (var slot in availableTimeSlotDtos)
            {
                slot.isBooked = false;
            }
            foreach (var slot in bookedTimeSlotDtos)
            {
                slot.isBooked = true;
            }

            List<ResponseTimeSlotWithStatusDTO> timeSlotList = new List<ResponseTimeSlotWithStatusDTO>();
            timeSlotList.AddRange(availableTimeSlotDtos);
            timeSlotList.AddRange(bookedTimeSlotDtos);
            var sortedTimeSlotList = timeSlotList.OrderBy(c => c.StartTime).ToList();

            return sortedTimeSlotList;
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

        public async Task ToggleStatusOfTimeSlot(string timeSlotId)
        {
            var timeSlot = await _timeSlotRepository.GetOneAsync(timeSlotId);
            if (timeSlot == null)
            {
                throw new NotFoundException("Time slot not found");
            }
            if (timeSlot.IsActive)
            {
                timeSlot.IsActive = false;
            }
            else
            {
                timeSlot.IsActive = true;
            }

            timeSlot.LastUpdatedTime = DateTime.UtcNow;
            _timeSlotRepository.Update(timeSlot);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
