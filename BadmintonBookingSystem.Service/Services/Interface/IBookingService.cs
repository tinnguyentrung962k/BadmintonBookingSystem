using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.Enum;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Service.Services.Interface
{
    public interface IBookingService
    {
        Task<BookingEntity>CreateBookingInSingleDay(string userId, SingleBookingCreateDTO singleBookingCreateDTO);
        Task<BookingEntity> CreateBookingFixed(string userId, FixedBookingCreateDTO fixedBookingCreateDTO);
        Task<BookingEntity> CreateBookingFlex(string userId, List<FlexBookingCreateDTO> flexBookingCreateDTOs);
        Task<IEnumerable<BookingDetailEntity>> GetAllBookingOfCustomersByCenterId(string centerId, int pageIndex, int pageSize);
        Task<IEnumerable<BookingDetailEntity>> SearchBookingOfCustomerByCenterId(string centerId, SearchBookingDTO searchBookingDTO, int pageIndex, int pageSize);
        Task<IEnumerable<BookingEntity>> GetAllBookingOfCustomerByUserId(string userId, int pageIndex, int pageSize);
        Task<IEnumerable<BookingEntity>> FilterStatusBookingOfCustomerByUserId(string userId, PaymentStatus? paymentStatus, int pageIndex, int pageSize);
        Task<BookingEntity> GetBookingById(string id);
        Task<BookingEntity> CancelBooking(string bookingId);
        Task<BookingDetailEntity> CancelBookingDetail(string bookingDetailId);

    }
}
