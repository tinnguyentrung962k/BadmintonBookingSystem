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
        public BookingService(IBookingRepository bookingRepository, UserManager<UserEntity> userManager, IUnitOfWork unitOfWork)
        {
            _bookingRepository = bookingRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateBookingInSingleDay(string userId, BookingEntity bookingEntity)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User Not Found");
            }
            var conflictSingleBooking = await _bookingRepository.QueryHelper()
                    .Filter(b => b.TimeSlotId.Equals(bookingEntity.TimeSlotId)
                    && b.BookingDate.Equals(bookingEntity.BookingDate))
                    .GetAllAsync();
            if (conflictSingleBooking.Any())
            {
                throw new ConflictException("Your chosen time slot has been booked!");
            }
            var conflictFixedBooking = await _bookingRepository.QueryHelper()
                .Filter(b => b.TimeSlotId.Equals(bookingEntity.TimeSlotId)
                && b.FromDate.HasValue && b.ToDate.HasValue
                && b.FromDate <= bookingEntity.BookingDate
                && bookingEntity.BookingDate <= b.ToDate
                && bookingEntity.BookingDate.Value.DayOfWeek.ToString().Equals(b.DayOfAWeek)
                && b.BookingType == BookingType.Fixed).GetAllAsync();

               
            if (conflictFixedBooking.Any())
            {
                throw new ConflictException("Your chosen time slot has fixed booking before!");
            }

            try
            {
                _unitOfWork.BeginTransaction();
                
                bookingEntity.CustomerId = userId;
                bookingEntity.DayOfAWeek = bookingEntity.BookingDate.Value.DayOfWeek.ToString();
                _bookingRepository.Add(bookingEntity);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("Error while handling booking");
            }
        }
    }
}
