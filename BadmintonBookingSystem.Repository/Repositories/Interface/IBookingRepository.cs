using BadmintonBookingSystem.BusinessObject.Enum;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Repository.Repositories.Interface
{
    public interface IBookingRepository : IGenericRepository<BookingEntity,string>
    {
        Task<IEnumerable<BookingEntity>> GetAUserBookings(string userId, int pageIndex, int pageSize);
        Task<IEnumerable<BookingEntity>> FilterStatusForAUserBookings(string userId, PaymentStatus? paymentStatus, int pageIndex, int pageSize);
        Task<BookingEntity> GetABookingById(string id);
    }
}
