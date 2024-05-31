using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Repository.Repositories.Interface;
using BadmintonBookingSystem.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Service.Services
{
    public class BadmintonCenterService : IBadmintonCenterService
    {
        private readonly IBadmintonCenterRepository _badmintonCenterRepository;
        public BadmintonCenterService(IBadmintonCenterRepository badmintonCenterRepository)
        {
            _badmintonCenterRepository = badmintonCenterRepository;
        }
        public async Task<IEnumerable<BadmintonCenterEntity>> GetAllBadmintonCenterAsync(int position, int size)
        {
            var badmintonCenter = await _badmintonCenterRepository.GetPagingAsync(position, size);
            if (!badmintonCenter.Any()) 
            {
                throw new NotFoundException("Empty list!");
            }
            return badmintonCenter;
        }
    }
}
