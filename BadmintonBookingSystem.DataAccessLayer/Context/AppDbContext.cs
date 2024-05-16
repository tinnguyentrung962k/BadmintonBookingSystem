using BadmintonBookingSystem.DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.DataAccessLayer.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(GetConnectionString());
        }
        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            var strConn = config["ConnectionString:DefaultConnectionStringDB"];
            return strConn;
        }


        public DbSet<BadmintonCenterEntity> BadmintonCenters { get; set; }
        public DbSet<BookingOrderEntity> BookingOrders { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<SlotEntity> Slots { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        

    }
}
