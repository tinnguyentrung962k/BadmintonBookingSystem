using Amazon.S3.Model;
using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs;
using BadmintonBookingSystem.BusinessObject.Enum;
using BadmintonBookingSystem.BusinessObject.Exceptions;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using BadmintonBookingSystem.Repository.Repositories.Interface;
using BadmintonBookingSystem.Service.Services.Interface;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Net.payOS;
using Net.payOS.Types;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BadmintonBookingSystem.Service.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookingService _bookingService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<UserEntity> _userManager;
        private readonly PayOsSetting payOSSetting;
        private PayOS _payOS;
        public PaymentService(IUnitOfWork unitOfWork, IBookingService bookingService, IConfiguration configuration, UserManager<UserEntity> userManager)
        {
            _unitOfWork = unitOfWork;
            _bookingService = bookingService;
            _configuration = configuration;
            _userManager = userManager;
            payOSSetting = new PayOsSetting()
            {
                ClientId = _configuration.GetValue<string>("PayOSClientId"),
                ApiKey = _configuration.GetValue<string>("PayOSApiKey"),
                ChecksumKey = _configuration.GetValue<string>("PayOSChecksumKey")
            };
            _payOS = new PayOS(payOSSetting.ClientId, payOSSetting.ApiKey, payOSSetting.ChecksumKey);
        }
        public async Task<string> PaymentWithPayOs(string bookingId)
        {
            var booking = await _bookingService.GetBookingById(bookingId);
            if (booking == null)
            {
                throw new NotFoundException("Booking not found");
            }

            List<ItemData> bookings = new List<ItemData>();
            var bookingDetails = booking.BookingDetails.ToList();
            var userId = booking.CustomerId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            foreach (var bookingDetail in bookingDetails)
            {
                var name = $"{bookingDetail.TimeSlot.Court.CourtName}-{bookingDetail.TimeSlot.StartTime}-{bookingDetail.TimeSlot.EndTime}-{bookingDetail.BookingDate}";
                var quantity = 1;
                var price = Convert.ToInt32(bookingDetail.TimeSlot.Price);
                ItemData item = new ItemData(name, quantity, price);
                bookings.Add(item);
            }

            string content = $"{user.FullName} - " + DateTime.Now.ToString();
            int expiredAt = (int)(DateTimeOffset.UtcNow.ToUnixTimeSeconds() + (60 * 5)); // 5 minutes from now

            long orderCodeLong = booking.BookingCode; // or use ConvertGuidToLong or ConvertGuidToLongUsingBase64

            PaymentData paymentData = new PaymentData(
                orderCodeLong,
                (int)booking.TotalPrice,
                content,
                bookings,
                "https://localhost:7138/api/payment/cancel",
                "https://localhost:7138/api/payment/return",
                null,
                user.FullName,
                user.Email,
                user.PhoneNumber,
                null,
                expiredAt
            );

            var createPayment = await _payOS.createPaymentLink(paymentData);
            if (createPayment == null || string.IsNullOrEmpty(createPayment.checkoutUrl))
            {
                throw new Exception("Failed to create payment link");
            }

            return createPayment.checkoutUrl;
        }
        public async Task<PaymentLinkInformation> GetPaymentStatus(string bookingId)
        {
            var booking = await _bookingService.GetBookingById(bookingId);
            if (booking == null)
            {
                throw new NotFoundException("Booking not found");
            }

            long orderCodeLong = booking.BookingCode;

            var paymentStatus = await _payOS.getPaymentLinkInformation(orderCodeLong);

            return paymentStatus;
        }

    }
}
