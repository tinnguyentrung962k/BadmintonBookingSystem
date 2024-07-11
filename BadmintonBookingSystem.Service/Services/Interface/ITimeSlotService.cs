using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Service.Services.Interface
{
    public interface ITimeSlotService
    {
        Task AddTimeSlotForCourt(string courtId);
        Task RemoveTimeSlotForCourt(string courtId);

    }
}
