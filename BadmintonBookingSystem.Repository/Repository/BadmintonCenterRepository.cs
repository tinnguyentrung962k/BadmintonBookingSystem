using BadmintonBookingSystem.DataAccessLayer.Context;
using BadmintonBookingSystem.DataAccessLayer.Entity;
using BadmintonBookingSystem.Repository.Base;
using BadmintonBookingSystem.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Repository.Repository
{
    public class BadmintonCenterRepository : BaseRepository<BadmintonCenterEntity>, IBadmintonCenterRepository
    {
        public BadmintonCenterRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
