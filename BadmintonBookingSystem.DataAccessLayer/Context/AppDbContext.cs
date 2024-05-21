using BadmintonBookingSystem.DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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

        public AppDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
            optionsBuilder.UseSqlServer(GetConnectionString());
            base.OnConfiguring(optionsBuilder);

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
        public DbSet<CourtEntity> Courts { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        //dotnet ef migrations add Init -s .\BadmintonBookingSystem -p .\BadmintonBookingSystem.DataAccessLayer
        //BadmintonBookingSystem.DataAccessLayer
        //dotnet ef database update --project <Project_contains_the_DBContext> --startup-project <Startup_roject>

    }
}
