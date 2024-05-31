using BadmintonBookingSystem.Service.Services;
using BadmintonBookingSystem.Service.Services.Interface;

namespace BadmintonBookingSystem.Configurations
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBadmintonCenterService, BadmintonCenterService>();
            return services;
        }
    }
}
