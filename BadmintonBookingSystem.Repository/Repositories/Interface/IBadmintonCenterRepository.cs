using BadmintonBookingSystem.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Repository.Repositories.Interface
{
    public interface IBadmintonCenterRepository : IGenericRepository<BadmintonCenterEntity,string>
    {
    }
}
