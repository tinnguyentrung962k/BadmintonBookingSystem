using BadmintonBookingSystem.DataAccessLayer.Entity;
using BadmintonBookingSystem.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Repository.Interface
{
    public interface IBadmintonCenterRepository : IBaseRepository<BadmintonCenterEntity> 
    {
    }
}
