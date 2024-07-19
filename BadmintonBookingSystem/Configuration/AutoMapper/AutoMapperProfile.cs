using AutoMapper;
using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using System.Data;

namespace BadmintonBookingSystem.Configuration.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            BadmintonCenterProfile();
            UserMappingProfile();
            CourtProfile();
            TimeSlotProfile();
            BookingProfile();
            RoleProfile();
        }
        private void BadmintonCenterProfile() {
            CreateMap<BadmintonCenterEntity, ResponseBadmintonCenterDTO>()
                .ForMember(rc=>rc.ManagerName, opt => opt.MapFrom(c=>c.Manager.FullName))
                .ForMember(rc => rc.ImgUrls,
                opt => opt.MapFrom(c => c.BadmintonCenterImages.Select(pi => pi.ImageLink).ToHashSet()))
                .ForMember(rc => rc.OperatingTime, opt => opt.MapFrom(src => src.OperatingTime.ToString("HH:mm")))
                .ForMember(rc => rc.ClosingTime, opt => opt.MapFrom(src => src.ClosingTime.ToString("HH:mm")))
                .ReverseMap();
            CreateMap<BadmintonCenterEntity, ResponseSearchBadmintonCenterDTO>();
            CreateMap<BadmintonCenterCreateDTO, BadmintonCenterEntity>();
            CreateMap<SearchBadmintonCenterDTO, BadmintonCenterEntity>();
            CreateMap<BadmintonUpdateDTO,BadmintonCenterEntity>();
        }
        private void UserMappingProfile()
        {
            CreateMap<UserEntity, RegisterDTO>().ReverseMap();
            CreateMap<UserEntity, ResponseUserDTO>()
                .ForMember(ru => ru.IsActive, opt => opt.MapFrom(u => u.EmailConfirmed))
                .ForMember(userDto => userDto.UserRoles,
                    opt => opt.MapFrom(user
                        => user.UserRoles.Select(iur => iur.Role.Name).ToHashSet()))
                .ReverseMap();
        }
        private void CourtProfile()
        {
            CreateMap<CourtEntity,ResponseCourtDTO>()
                .ForMember(rcourt => rcourt.CenterName, opt => opt.MapFrom(court => court.BadmintonCenter.Name))
                .ForMember(rcourt => rcourt.CenterId, opt => opt.MapFrom(court => court.BadmintonCenter.Id))
                .ForMember(rc => rc.ImgUrls,
                opt => opt.MapFrom(c => c.CourtImages.Select(pi => pi.ImageLink).ToHashSet()))
                .ReverseMap();
            CreateMap<CourtEntity,CourtCreateDTO>().ReverseMap();
            CreateMap<CourtEntity,CourtUpdateDTO>().ReverseMap();
        }
        private void TimeSlotProfile()
        {
            CreateMap<TimeSlotCreateDTO, TimeSlotEntity>();
            CreateMap<TimeSlotEntity, ResponseTimeSlotDTO>()
                .ForMember(rts => rts.StartTime, opt => opt.MapFrom(ts => ts.StartTime.ToString("HH:mm")))
                .ForMember(rts => rts.EndTime, opt => opt.MapFrom(ts => ts.EndTime.ToString("HH:mm")))
                .ForMember(rts => rts.CourtName, opt => opt.MapFrom(ts => ts.Court.CourtName));
            CreateMap<TimeSlotEntity, ResponseTimeSlotWithStatusDTO>()
                .ForMember(rts => rts.CourtName, opt => opt.MapFrom(ts => ts.Court.CourtName));

        }
        private void BookingProfile() 
        {
            CreateMap<SingleBookingCreateDTO, BookingEntity>().ReverseMap();
            CreateMap<BookingEntity, ResponseBookingHeaderDTO>()
                .ForMember(c => c.CustomerId, opt => opt.MapFrom(c => c.Customer.Id))
                .ForMember(c => c.CustomerName, opt => opt.MapFrom(c => c.Customer.FullName))
                .ForMember(c => c.CustomerEmail, opt => opt.MapFrom(c => c.Customer.Email))
                .ForMember(c => c.CustomerPhone, opt => opt.MapFrom(c => c.Customer.PhoneNumber));
            CreateMap<BookingDetailEntity, ResponseBookingDetailDTO>()
                .ForMember(c => c.StartTime, opt => opt.MapFrom(c => c.TimeSlot.StartTime))
                .ForMember(c => c.EndTime, opt => opt.MapFrom(c => c.TimeSlot.EndTime));
            CreateMap<BookingEntity, ResponseBookingHeaderAndBookingDetail>()
                .ForMember(c=>c.BookingHeader, opt => opt.MapFrom(c => c))
                .ForMember(c=>c.BookingDetails,opt => opt.MapFrom(c => c.BookingDetails));
        }
        private void RoleProfile()
        {
            CreateMap<RoleEntity,RoleResponseDTO>()
                .ForMember(c => c.RoleName, opt => opt.MapFrom(c=>c.Name));
        }
    }
}
