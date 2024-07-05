using BadmintonBookingSystem.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Service.Services.Interface
{
    public interface ICourtService
    {
        Task<IEnumerable<CourtEntity>> GetAllCourtByCenterId(string centerId,int pageIndex, int size);
        Task<CourtEntity> GetCourtById(string courtId);
        Task CreateNewCourt(CourtEntity entity);
        Task UpdateCourt (CourtEntity entity);
    }
}
