using BadmintonBookingSystem.DataAccessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Service.Interface
{
    public interface IBadmintonCenterService
    {
        public IQueryable<BadmintonCenterEntity> GetAllBadmintonCenters();
    }
}
