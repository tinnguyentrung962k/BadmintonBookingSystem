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
    public class CourtRepository : GenericRepository<CourtEntity, string>, ICourtRepository
    {
        public CourtRepository(AppDbContext context) : base(context)
        {
        }
    }
}
