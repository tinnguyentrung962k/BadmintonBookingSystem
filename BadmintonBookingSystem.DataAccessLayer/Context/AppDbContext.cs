﻿using BadmintonBookingSystem.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BadmintonBookingSystem.DataAccessLayer.Context
{
    public class AppDbContext : IdentityDbContext<UserEntity, RoleEntity, string,
        IdentityUserClaim<string>,
        UserRoleEntity,
        IdentityUserLogin<string>,
        IdentityRoleClaim<string>,
        IdentityUserToken<string>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<BadmintonCenterEntity> BadmintonCenters { get; set; }
        public DbSet<BookingOrderEntity> BookingOrders { get; set; }
        public DbSet<CourtEntity> Courts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRoleEntity>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("User");
            });

            modelBuilder.Entity<RoleEntity>(entity =>
            {
                entity.ToTable("Role");
            });

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName!.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
        }

        //dotnet ef migrations add Init -s .\BadmintonBookingSystem -p .\BadmintonBookingSystem.DataAccessLayer
        //BadmintonBookingSystem.DataAccessLayer
        //dotnet ef database update --project <Project_contains_the_DBContext> --startup-project <Startup_roject>

    }
}
