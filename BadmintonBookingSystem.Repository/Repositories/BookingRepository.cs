using BadmintonBookingSystem.BusinessObject.Enum;
using BadmintonBookingSystem.DataAccessLayer.Context;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Repository.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Repository.Repositories
{
    public class BookingRepository : GenericRepository<BookingEntity, string>, IBookingRepository
    {
        private readonly AppDbContext _appDbContext;
        public BookingRepository(AppDbContext context) : base(context)
        {
            _appDbContext = context;
        }

        public async Task<IEnumerable<BookingEntity>> FilterStatusForAUserBookings(string userId, PaymentStatus? paymentStatus, int pageIndex, int pageSize)
        {
            var query = _appDbContext.Bookings.Where(c => c.CustomerId.Equals(userId));
            if (paymentStatus != null)
            {
                query = query.Where(c => c.PaymentStatus == paymentStatus);
            }
            var userBookings = await query
                .Include(c => c.Customer)                    
                .Include(c => c.BookingDetails)
                .ThenInclude(d => d.TimeSlot)
                .OrderByDescending(c => c.CreatedTime).ToListAsync();

            pageIndex = pageIndex < 1 ? 0 : pageIndex - 1;
            pageSize = pageSize < 1 ? 10 : pageSize;
            var pagedBookings = userBookings.Skip(pageIndex * pageSize).Take(pageSize);
            return pagedBookings;
        }

        public async Task<BookingEntity> GetABookingById(string id)
        {
            var userBooking = await _appDbContext.Bookings.Where(c=>c.Id.Equals(id))
                .Include(c => c.Customer)
                .Include(c => c.BookingDetails)
                .ThenInclude(d => d.TimeSlot)
                .FirstOrDefaultAsync();
            return userBooking;
        }

        public async Task<IEnumerable<BookingEntity>> GetAUserBookings(string userId, int pageIndex, int pageSize)
        {
            var userBookings = await _appDbContext.Bookings.Where(c => c.CustomerId.Equals(userId))
                .Include(c=>c.Customer)
                .Include(c => c.BookingDetails)
                .ThenInclude(d => d.TimeSlot)
                .OrderByDescending(c => c.CreatedTime).ToListAsync();
            
            pageIndex = pageIndex < 1 ? 0 : pageIndex - 1;
            pageSize = pageSize < 1 ? 10 : pageSize;
            var pagedBookings = userBookings.Skip(pageIndex * pageSize).Take(pageSize);
            return pagedBookings;
        }
    }
}
