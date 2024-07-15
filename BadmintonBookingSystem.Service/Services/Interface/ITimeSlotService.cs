using BadmintonBookingSystem.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Service.Services.Interface
{
    public interface ITimeSlotService
    {
        Task<IEnumerable<TimeSlotEntity>> GetAllTimeSlotsByCourtId(string courtId, int pageIndex, int pageSize);
        Task<TimeSlotEntity> CreateATimeSlot(TimeSlotEntity timeSlotEntity);
        Task<TimeSlotEntity> GetTimeSlotById(string id);
    }
}
