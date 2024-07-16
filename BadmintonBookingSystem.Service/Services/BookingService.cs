using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.Enum;
using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Repository.Repositories.Interface;
using BadmintonBookingSystem.Service.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;


namespace BadmintonBookingSystem.Service.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookingDetailRepository _bookDetailRepository;
        public BookingService(IBookingRepository bookingRepository, UserManager<UserEntity> userManager, IUnitOfWork unitOfWork, IBookingDetailRepository bookingDetailRepository)
        {
            _bookingRepository = bookingRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _bookDetailRepository = bookingDetailRepository;    
        }

        public async Task CreateBookingInSingleDay(string userId, SingleBookingCreateDTO singleBookingCreateDTO)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User Not Found!");
            }
            foreach (var timeSlotId in singleBookingCreateDTO.ListTimeSlotId)
            {
                var existedBookingDetail = await _bookDetailRepository.QueryHelper()
                .Filter(bd => bd.TimeSlotId.Equals(timeSlotId)
                && bd.BookingDate.Equals(singleBookingCreateDTO.BookingDate))
                .Include(bd => bd.TimeSlot)
                .GetOneAsync();

                if (existedBookingDetail != null)
                {
                    var formatDate = singleBookingCreateDTO.BookingDate.ToString("dd/MM/yyyy");
                    throw new ConflictException($"Time slot from {existedBookingDetail.TimeSlot.StartTime} to {existedBookingDetail.TimeSlot.EndTime} on {formatDate} is booked !");
                }
                break;
            }
            try
            {
                _unitOfWork.BeginTransaction();
                BookingEntity booking = new BookingEntity
                {
                    BookingType = BookingType.Single,
                    CustomerId = user.Id,
                    FromDate = singleBookingCreateDTO.BookingDate,
                    ToDate = singleBookingCreateDTO.BookingDate
                };

                _bookingRepository.Add(booking);

                foreach (var timeSlotId in singleBookingCreateDTO.ListTimeSlotId)
                {
                    var bookingDetail = new BookingDetailEntity
                    {
                        BookingId = booking.Id,
                        DateOfWeek = singleBookingCreateDTO.BookingDate.DayOfWeek.ToString(),
                        TimeSlotId = timeSlotId,
                        BookingDate = singleBookingCreateDTO.BookingDate
                    };
                    _bookDetailRepository.Add(bookingDetail);
                }
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw ex;
            }
        }
    }
    
}
