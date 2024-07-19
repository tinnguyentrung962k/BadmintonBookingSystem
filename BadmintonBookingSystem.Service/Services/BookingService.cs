using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.Enum;
using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Repository.Repositories;
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
        private readonly ITimeSlotRepository _timeSlotRepository;
        public BookingService(IBookingRepository bookingRepository, UserManager<UserEntity> userManager, IUnitOfWork unitOfWork, IBookingDetailRepository bookingDetailRepository, ITimeSlotRepository timeSlotRepository)
        {
            _bookingRepository = bookingRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _bookDetailRepository = bookingDetailRepository;
            _timeSlotRepository = timeSlotRepository;
        }

        public async Task<BookingEntity> CreateBookingInSingleDay(string userId, SingleBookingCreateDTO singleBookingCreateDTO)
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
                    ToDate = singleBookingCreateDTO.BookingDate,
                    Customer = user,
                    
                };

                _bookingRepository.Add(booking);

                foreach (var timeSlotId in singleBookingCreateDTO.ListTimeSlotId)
                {
                    var bookingDetail = new BookingDetailEntity
                    {
                        BookingId = booking.Id,
                        DayOfAWeek = (DayOfAWeek)singleBookingCreateDTO.BookingDate.DayOfWeek,
                        TimeSlotId = timeSlotId,
                        TimeSlot = await _timeSlotRepository.GetOneAsync(timeSlotId),
                        BookingDate = singleBookingCreateDTO.BookingDate,
                        Booking = booking

                    };
                    _bookDetailRepository.Add(bookingDetail);
                }
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return booking;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw ex;
            }
        }
        public async Task<BookingEntity> CreateBookingFixed(string userId, FixedBookingCreateDTO fixedBookingCreateDTO)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User Not Found!");
            }

            var bookingDates = new List<DateOnly>();
            for (var date = fixedBookingCreateDTO.FromDate; date <= fixedBookingCreateDTO.ToDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek == (DayOfWeek)(((int)fixedBookingCreateDTO.DayOfAWeek) % 7))
                {
                    bookingDates.Add(date);
                }
            }

            foreach (var timeSlotId in fixedBookingCreateDTO.ListTimeSlotId)
            {
                var existedBookingDetail = await _bookDetailRepository.QueryHelper()
                .Filter(bd => bd.TimeSlotId.Equals(timeSlotId)
                && bd.BookingDate >= fixedBookingCreateDTO.FromDate 
                && bd.BookingDate <= fixedBookingCreateDTO.ToDate
                && bd.DayOfAWeek.Equals(fixedBookingCreateDTO.DayOfAWeek))
                .Include(bd => bd.TimeSlot)
                .GetOneAsync();

                if (existedBookingDetail != null)
                {
                    var formatDate = existedBookingDetail.BookingDate.ToString("dd/MM/yyyy");
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
                    FromDate = fixedBookingCreateDTO.FromDate,
                    ToDate = fixedBookingCreateDTO.ToDate,
                    Customer = user,

                };

                _bookingRepository.Add(booking);

                foreach (var timeSlotId in fixedBookingCreateDTO.ListTimeSlotId)
                {
                    foreach (var bookingDate in bookingDates)
                    {
                        var bookingDetail = new BookingDetailEntity
                        {
                            BookingId = booking.Id,
                            DayOfAWeek = fixedBookingCreateDTO.DayOfAWeek,
                            TimeSlotId = timeSlotId,
                            TimeSlot = await _timeSlotRepository.GetOneAsync(timeSlotId),
                            BookingDate = bookingDate,
                            Booking = booking

                        };
                        _bookDetailRepository.Add(bookingDetail);
                    }
                        
                }
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return booking;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw ex;
            }

        }
        
    }
    
}
