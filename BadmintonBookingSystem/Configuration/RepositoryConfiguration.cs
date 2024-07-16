using BadmintonBookingSystem.Repository.Repositories;
using BadmintonBookingSystem.Repository.Repositories.Interface;
using BadmintonBookingSystem.Service.Services.Interface;
using BadmintonBookingSystem.Service.Services;

namespace BadmintonBookingSystem.Configuration
{
    public static class RepositoryConfiguration
    {
        public static IServiceCollection AddRepositoryConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBadmintonCenterRepository, BadmintonCenterRepository>();
            services.AddScoped<ICourtRepository, CourtRepository>();
            services.AddScoped<ITimeSlotRepository, TimeSlotRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IBookingDetailRepository, BookingDetailRepository>();

            return services;
        }
    }
}
