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
        public long GenerateUniqueBookingCode()
        {
            var random = new Random();
            long bookingCode;

            // Generate a random number with a desired number of digits, e.g., 6 digits
            bookingCode = random.Next(100000, 999999);

            // Ensure the booking code is unique
            while (_bookingRepository.ExistsAsync(bookingCode).Result)
            {
                bookingCode = random.Next(100000, 999999);
            }

            return bookingCode;
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
                    .Filter(bd => bd.TimeSlotId.Equals(timeSlotId) && bd.BookingDate.Equals(singleBookingCreateDTO.BookingDate) && bd.ReservationStatus != ReservationStatus.Cancelled)
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
                    BookingCode = GenerateUniqueBookingCode(),
                    BookingType = BookingType.Single,
                    CustomerId = user.Id,
                    FromDate = singleBookingCreateDTO.BookingDate,
                    ToDate = singleBookingCreateDTO.BookingDate,
                    Customer = user,
                    TotalPrice = totalPrice,
                    PaymentStatus = PaymentStatus.NotPaid
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
                            && bd.DayOfAWeek.Equals(fixedBookingCreateDTO.DayOfAWeek) && bd.ReservationStatus != ReservationStatus.Cancelled)
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
                    BookingCode = GenerateUniqueBookingCode(),
                    BookingType = BookingType.Fixed,
                    CustomerId = user.Id,
                    FromDate = fixedBookingCreateDTO.FromDate,
                    ToDate = fixedBookingCreateDTO.ToDate,
                    Customer = user,
                    PaymentStatus = PaymentStatus.NotPaid,
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
                .Filter(c => c.TimeSlot.Court.CenterId.Equals(centerId))
                .Include(c => c.Booking.Customer)
                .Include(c => c.TimeSlot.Court)
                .Include(c=>c.TimeSlot.Court.BadmintonCenter)
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
                throw new NotFoundException("Center not found");
            }
            var search = _bookDetailRepository.QueryHelper()
                .Filter(c => c.TimeSlot.Court.CenterId.Equals(centerId))
                .OrderBy(c => c.OrderByDescending(c => c.BookingDate))
                .Include(c => c.Booking.Customer)
                .Include(c => c.TimeSlot.Court)
                .Include(c => c.TimeSlot.Court.BadmintonCenter)
                .Include(c => c.TimeSlot);
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

            return await search.GetPagingAsync(pageIndex, pageSize);
        }
        public async Task<BookingEntity> CreateBookingFlex(string userId, List<FlexBookingCreateDTO> flexBookingCreateDTOs)
        {
            // Validate user
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User Not Found!");
            }

            // Validate time slots for conflicts
            foreach (var chosenTimeSlot in flexBookingCreateDTOs)
            {
                foreach (var timeSlotId in chosenTimeSlot.ListTimeSlotId)
                {
                    var existedBookingDetail = await _bookDetailRepository.QueryHelper()
                        .Filter(bd => bd.TimeSlotId == timeSlotId
                            && bd.BookingDate == chosenTimeSlot.BookingDate
                            && bd.DayOfAWeek == (DayOfAWeek)chosenTimeSlot.BookingDate.DayOfWeek && bd.ReservationStatus != ReservationStatus.Cancelled)
                        .GetOneAsync(); // Using AnyAsync to improve performance

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

                // Create BookingEntity
                var bookingEntity = new BookingEntity
                {
                    BookingCode = GenerateUniqueBookingCode(),
                    CustomerId = userId,
                    Customer = user,
                    BookingType = BookingType.Flexible,
                    FromDate = flexBookingCreateDTOs.Min(ts => ts.BookingDate),
                    ToDate = flexBookingCreateDTOs.Max(ts => ts.BookingDate),
                    PaymentStatus = PaymentStatus.NotPaid
                };

                decimal totalPrice = 0;
                foreach (var chosenTimeSlot in flexBookingCreateDTOs)
                {
                    foreach (var timeSlotId in chosenTimeSlot.ListTimeSlotId)
                    {
                        var timeSlot = await _timeSlotRepository.GetOneAsync(timeSlotId);
                        if (timeSlot == null)
                        {
                            throw new NotFoundException($"TimeSlot with ID {timeSlotId} Not Found!");
                        }

                        var bookingDetailEntity = new BookingDetailEntity
                        {
                            TimeSlotId = timeSlotId,
                            TimeSlot = timeSlot,
                            BookingDate = chosenTimeSlot.BookingDate,
                            DayOfAWeek = (DayOfAWeek)chosenTimeSlot.BookingDate.DayOfWeek,
                            ReservationStatus = ReservationStatus.NotCheckedIn,
                            Booking = bookingEntity
                        };

                        _bookDetailRepository.Add(bookingDetailEntity);
                        totalPrice += timeSlot.Price;
                    }
                }

                bookingEntity.TotalPrice = totalPrice;
                _bookingRepository.Add(bookingEntity);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return bookingEntity;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                // Log exception if needed
                throw; // Rethrow the original exception
            }
        }

        public async Task<IEnumerable<BookingEntity>> GetAllBookingOfCustomerByUserId(string userId, int pageIndex, int pageSize)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("No user found");
            }
            var bookingOrders = await _bookingRepository.GetAUserBookings(userId,pageIndex,pageSize);

            if (!bookingOrders.Any())
            {
                throw new NotFoundException("No booking found !");
            }

            return bookingOrders;

        }

        public async Task<IEnumerable<BookingEntity>> FilterStatusBookingOfCustomerByUserId(string userId, PaymentStatus? paymentStatus, int pageIndex, int pageSize)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("No user found");
            }
            var bookingOrders = await _bookingRepository.FilterStatusForAUserBookings(userId, paymentStatus, pageIndex, pageSize);

            if (!bookingOrders.Any())
            {
                throw new NotFoundException("No booking found !");
            }

            return bookingOrders;
        }

        public async Task<BookingEntity> GetBookingById(string id)
        {
            var chosenBooking = await _bookingRepository.GetABookingById(id);
            if (chosenBooking is null)
            {
                throw new NotFoundException("Bookings not found !");
            }
            return chosenBooking;
        }

        public async Task<BookingEntity> CancelBooking(string bookingId)
        {
            var chosenBooking = await _bookingRepository.GetABookingById(bookingId);
            if (chosenBooking is null)
            {
                throw new NotFoundException("No booking found");
            }
            try
            {
                chosenBooking.PaymentStatus = PaymentStatus.Cancel;
                var bookingDetails = await _bookDetailRepository.QueryHelper()
                    .Filter(c => c.BookingId.Equals(bookingId))
                    .GetAllAsync();
                foreach (var bookingDetail in bookingDetails)
                {
                    bookingDetail.ReservationStatus = ReservationStatus.Cancelled;
                    _bookDetailRepository.Update(bookingDetail);
                }
                _bookingRepository.Update(chosenBooking);
                await _unitOfWork.SaveChangesAsync();
                return chosenBooking;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw ex;
            }
        }
        public async Task<BookingEntity> CompleteBooking(string bookingId)
        {
            var chosenBooking = await _bookingRepository.GetABookingById(bookingId);
            if (chosenBooking is null)
            {
                throw new NotFoundException("No booking found");
            }
            try
            {
                chosenBooking.PaymentStatus = PaymentStatus.Completed;
                var bookingDetails = await _bookDetailRepository.QueryHelper()
                    .Filter(c => c.BookingId.Equals(bookingId))
                    .GetAllAsync();
                _bookingRepository.Update(chosenBooking);
                await _unitOfWork.SaveChangesAsync();
                return chosenBooking;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw ex;
            }
        }

        public async Task<BookingDetailEntity> CancelBookingDetail(string bookingDetailId)
        {
            var chosenBookingDetail = await _bookDetailRepository.QueryHelper()
                .Filter(c => c.Id.Equals(bookingDetailId))
                .Include(c => c.Booking.Customer)
                .Include(c => c.TimeSlot.Court)
                .Include(c => c.TimeSlot.Court.BadmintonCenter)
                .Include(c => c.TimeSlot).GetOneAsync();
            if (chosenBookingDetail == null)
            {
                throw new NotFoundException("No booking detail found");
            }
            try
            {
                chosenBookingDetail.ReservationStatus = ReservationStatus.Cancelled;
                _bookDetailRepository.Update(chosenBookingDetail);
                await _unitOfWork.SaveChangesAsync();
                return chosenBookingDetail;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw ex;
            }
            

        }

        public async Task<BookingEntity> GetBookingByOrderCode(long orderCode)
        {
            var chosenBooking = await _bookingRepository.QueryHelper()
                .Filter(c=>c.BookingCode == orderCode)
                .GetOneAsync();
            if (chosenBooking == null)
            {
                throw new NotFoundException("Booking not found");
            }
            return chosenBooking;
        }

        public async Task<BookingDetailEntity> UpdateStatusReservation(string bookingDetailId, ReservationStatus status)
        {
            var chosenBookingDetail = await _bookDetailRepository.QueryHelper()
                .Filter(c => c.Id.Equals(bookingDetailId))
                .Include(c => c.Booking.Customer)
                .Include(c => c.TimeSlot.Court)
                .Include(c => c.TimeSlot.Court.BadmintonCenter)
                .Include(c => c.TimeSlot).GetOneAsync();
            if (chosenBookingDetail == null)
            {
                throw new NotFoundException("No booking detail found");
            }
            try {
                chosenBookingDetail.ReservationStatus = status;
                _bookDetailRepository.Update(chosenBookingDetail);
                await _unitOfWork.SaveChangesAsync();
                return chosenBookingDetail;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw ex;
            }

        }
    }
}
