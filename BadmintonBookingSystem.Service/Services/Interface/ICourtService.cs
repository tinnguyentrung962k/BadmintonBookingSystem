using BadmintonBookingSystem.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Http;
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
        Task<IEnumerable<CourtEntity>> GetAllActiveCourtsByCenterId(string centerId, int pageIndex, int size);
        Task<CourtEntity> GetCourtById(string courtId);
        Task CreateNewCourt(CourtEntity entity, List<IFormFile> picList);
        Task<CourtEntity> UpdateCourt (CourtEntity entity,string courtId, List<IFormFile> newPicList);
        Task ToggleStatusCourt(string courtId);
    }
}
