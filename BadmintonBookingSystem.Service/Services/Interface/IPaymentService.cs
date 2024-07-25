using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.Service.Services.Interface
{
    public interface IPaymentService
    {
        Task<string> PaymentWithPayOs(string bookingId);
        Task<PaymentLinkInformation> GetPaymentStatus(string bookingId);
    }
}
