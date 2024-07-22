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
        private readonly IBadmintonCenterRepository _badmintonCenterRepository;
        public BookingService(IBookingRepository bookingRepository, UserManager<UserEntity> userManager, IUnitOfWork unitOfWork, IBookingDetailRepository bookingDetailRepository, ITimeSlotRepository timeSlotRepository, IBadmintonCenterRepository badmintonCenterRepository)
        {
            _bookingRepository = bookingRepository;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _bookDetailRepository = bookingDetailRepository;
            _timeSlotRepository = timeSlotRepository;
            _badmintonCenterRepository = badmintonCenterRepository;
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
                    .Filter(bd => bd.TimeSlotId.Equals(timeSlotId) && bd.BookingDate.Equals(singleBookingCreateDTO.BookingDate))
                    .Include(bd => bd.TimeSlot)
                    .GetOneAsync();

                if (existedBookingDetail != null)
                {
                    var formatDate = singleBookingCreateDTO.BookingDate.ToString("dd/MM/yyyy");
                    throw new ConflictException($"Time slot from {existedBookingDetail.TimeSlot.StartTime} to {existedBookingDetail.TimeSlot.EndTime} on {formatDate} is booked!");
                }
                break;
            }

            try
            {
                _unitOfWork.BeginTransaction();

                decimal totalPrice = 0;

                foreach (var timeSlotId in singleBookingCreateDTO.ListTimeSlotId)
                {
                    var timeSlot = await _timeSlotRepository.GetOneAsync(timeSlotId);
                    if (timeSlot == null)
                    {
                        throw new NotFoundException("TimeSlot Not Found!");
                    }
                    totalPrice += timeSlot.Price;
                }

                BookingEntity booking = new BookingEntity
                {
                    BookingType = BookingType.Single,
                    CustomerId = user.Id,
                    FromDate = singleBookingCreateDTO.BookingDate,
                    ToDate = singleBookingCreateDTO.BookingDate,
                    Customer = user,
                    TotalPrice = totalPrice
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
                        Booking = booking,
                        ReservationStatus = ReservationStatus.NotCheckedIn
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
                foreach (var bookingDate in bookingDates)
                {
                    var existedBookingDetail = await _bookDetailRepository.QueryHelper()
                        .Filter(bd => bd.TimeSlotId.Equals(timeSlotId)
                            && bd.BookingDate.Equals(bookingDate)
                            && bd.DayOfAWeek.Equals(fixedBookingCreateDTO.DayOfAWeek))
                        .Include(bd => bd.TimeSlot)
                        .GetOneAsync();

                    if (existedBookingDetail != null)
                    {
                        var formatDate = existedBookingDetail.BookingDate.ToString("dd/MM/yyyy");
                        throw new ConflictException($"Time slot from {existedBookingDetail.TimeSlot.StartTime} to {existedBookingDetail.TimeSlot.EndTime} on {formatDate} is booked!");
                    }
                }
            }

            try
            {
                _unitOfWork.BeginTransaction();

                decimal totalPrice = 0;

                // Calculate total price for the booking
                foreach (var timeSlotId in fixedBookingCreateDTO.ListTimeSlotId)
                {
                    var timeSlot = await _timeSlotRepository.GetOneAsync(timeSlotId);
                    if (timeSlot == null)
                    {
                        throw new NotFoundException("TimeSlot Not Found!");
                    }
                    totalPrice += timeSlot.Price * bookingDates.Count;
                }

                BookingEntity booking = new BookingEntity
                {
                    BookingType = BookingType.Fixed,
                    CustomerId = user.Id,
                    FromDate = fixedBookingCreateDTO.FromDate,
                    ToDate = fixedBookingCreateDTO.ToDate,
                    Customer = user,
                    TotalPrice = totalPrice // Assuming BookingEntity has a TotalPrice property
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
                            Booking = booking,
                            ReservationStatus = ReservationStatus.NotCheckedIn
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

        public async Task<IEnumerable<BookingDetailEntity>> GetAllBookingOfCustomersByCenterId(string centerId, int pageIndex, int pageSize)
        {
            var center = await _badmintonCenterRepository.GetOneAsync(centerId);
            if (center == null)
            {
                throw new Exception("Center not found");
            }
            var bookingDetails = await _bookDetailRepository.QueryHelper()
                .Filter(c=>c.TimeSlot.Court.CenterId.Equals(centerId))
                .Include(c => c.Booking.Customer)
                .Include(c => c.TimeSlot.Court)
                .Include(c => c.TimeSlot)
                .OrderBy(c => c.OrderByDescending(c => c.BookingDate))
                .GetPagingAsync(pageIndex, pageSize);

            if (!bookingDetails.Any())
            {
                throw new NotFoundException("No bookings found for this center !");
            }

            return bookingDetails;
        }

        public async Task<IEnumerable<BookingDetailEntity>> SearchBookingOfCustomerByCenterId(string centerId, SearchBookingDTO searchBookingDTO, int pageIndex, int pageSize)
        {
            var center = await _badmintonCenterRepository.GetOneAsync(centerId);
            if (center == null)
            {
                throw new Exception("Center not found");
            }
            var search = _bookDetailRepository.QueryHelper()
                .OrderBy(c=>c.OrderByDescending(c=>c.BookingDate))
                .Include(c => c.Booking.Customer)
                .Include(c => c.TimeSlot.Court)
                .Include(c => c.TimeSlot);
            if (!string.IsNullOrEmpty(searchBookingDTO.BookingId))
            {
                search = search.Filter(bd => bd.BookingId.Contains(searchBookingDTO.BookingId));
            }
            if (!string.IsNullOrEmpty(searchBookingDTO.CustomerName))
            {
                search = search.Filter(bd => bd.Booking.Customer.FullName.ToLower().Contains(searchBookingDTO.CustomerName.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchBookingDTO.CustomerPhone))
            {
                search = search.Filter(bd => bd.Booking.Customer.PhoneNumber.ToLower().Contains(searchBookingDTO.CustomerPhone.ToLower()));
            }
            if (!string.IsNullOrEmpty(searchBookingDTO.CustomerEmail))
            {
                search = search.Filter(bd => bd.Booking.Customer.Email.ToLower().Contains(searchBookingDTO.CustomerEmail.ToLower()));
            }
            if (searchBookingDTO.FromDate != default || searchBookingDTO.ToDate != default)
            {
                if (searchBookingDTO.FromDate != default)
                {
                    search = search.Filter(bd => bd.BookingDate >= searchBookingDTO.FromDate);
                }
                if (searchBookingDTO.ToDate != default)
                {
                    search = search.Filter(bd => bd.BookingDate <= searchBookingDTO.ToDate);
                }
            }
            if (searchBookingDTO.FromDate != default && searchBookingDTO.ToDate != default)
            {
                search = search.Filter(bd => bd.BookingDate >= searchBookingDTO.FromDate && bd.BookingDate <= searchBookingDTO.ToDate);
            }

            return await search.GetPagingAsync(pageIndex,pageSize);
        }
    }
    
}
