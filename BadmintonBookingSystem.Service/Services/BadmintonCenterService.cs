using BadmintonBookingSystem.DataAccessLayer.Entity;
using BadmintonBookingSystem.Repository.Interface;
using BadmintonBookingSystem.Service.Interface;
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
        public IQueryable<BadmintonCenterEntity> GetAllBadmintonCenters()
        {
            var badmintonCenterList = _badmintonCenterRepository.GetAll();
            return badmintonCenterList;
        }
    }
}
