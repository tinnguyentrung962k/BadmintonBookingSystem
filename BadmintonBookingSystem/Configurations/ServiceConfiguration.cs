using BadmintonBookingSystem.Service.Services;
using BadmintonBookingSystem.Service.Services.Interface;

namespace BadmintonBookingSystem.Configurations
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IBadmintonCenterService, BadmintonCenterService>();
            return services;
        }
    }
}
