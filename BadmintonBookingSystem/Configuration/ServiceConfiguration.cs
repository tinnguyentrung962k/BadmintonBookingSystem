﻿using BadmintonBookingSystem.Service.Services;
using BadmintonBookingSystem.Service.Services.Interface;

namespace BadmintonBookingSystem.Configuration
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IBadmintonCenterService, BadmintonCenterService>();
            services.AddScoped<ICourtService, CourtService>();
            services.AddScoped<IAWSS3Service, AWSS3Service>();
            services.AddScoped<ITimeSlotService, TimeSlotService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IPaymentService, PaymentService>();
            return services;
        }
    }
}
