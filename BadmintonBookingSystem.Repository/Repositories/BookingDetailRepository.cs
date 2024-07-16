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
    public class BookingDetailRepository : GenericRepository<BookingDetailEntity, string>, IBookingDetailRepository
    {
        public BookingDetailRepository(AppDbContext context) : base(context)
        {
        }
    }
}
