using BadmintonBookingSystem.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Service.Services.Interface
{
    public interface IBadmintonCenterService
    {
        Task<IEnumerable<BadmintonCenterEntity>> GetAllBadmintonCenterAsync(int pageIndex, int size);
        Task CreateBadmintonCenter(BadmintonCenterEntity badmintonCenterEntity);
        Task<BadmintonCenterEntity> GetBadmintonCenterByIdAsync(string centerId);
        Task<BadmintonCenterEntity> UpdateBadmintonInfo(BadmintonCenterEntity badmintonCenterEntity, string centerId);
    }
}
