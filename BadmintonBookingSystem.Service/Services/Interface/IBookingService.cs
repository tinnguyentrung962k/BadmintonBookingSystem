using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
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

    }
}
