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
        }
        private void BadmintonCenterProfile() {
            CreateMap<BadmintonCenterEntity, ResponseBadmintonCenterDTO>()
                .ForMember(rc=>rc.ManagerName, opt => opt.MapFrom(c=>c.Manager.FullName))
                .ForMember(rc => rc.ImgUrls,
                opt => opt.MapFrom(c => c.BadmintonCenterImages.Select(pi => pi.ImageLink).ToHashSet()))
                .ReverseMap();
            CreateMap<BadmintonCenterEntity, BadmintonCenterCreateDTO>().ReverseMap();
            CreateMap<BadmintonCenterEntity, BadmintonUpdateDTO>().ReverseMap();
        }
        private void UserMappingProfile()
        {
            CreateMap<UserEntity, RegisterDTO>().ReverseMap();
            CreateMap<UserEntity, ResponseUserDTO>().ReverseMap();
        }
        private void CourtProfile()
        {
            CreateMap<CourtEntity,ResponseCourtDTO>()
                .ForMember(rcourt => rcourt.CenterName, opt => opt.MapFrom(court => court.BadmintonCenter.Name))
                .ForMember(rc => rc.ImgUrls,
                opt => opt.MapFrom(c => c.CourtImages.Select(pi => pi.ImageLink).ToHashSet()))
                .ReverseMap();
            CreateMap<CourtEntity,CourtCreateDTO>().ReverseMap();
            CreateMap<CourtEntity,CourtUpdateDTO>().ReverseMap();
        }
    }
}
