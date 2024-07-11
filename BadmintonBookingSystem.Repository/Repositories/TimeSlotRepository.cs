using BadmintonBookingSystem.DataAccessLayer.Context;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Repository.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Repository.Repositories
{
    public class TimeSlotRepository : GenericRepository<TimeSlotEntity, string>, ITimeSlotRepository
    {
        public TimeSlotRepository(AppDbContext context) : base(context)
        {
        }
    }
}
