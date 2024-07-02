using AutoMapper;
using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs;
using BadmintonBookingSystem.DataAccessLayer.Entities;

namespace BadmintonBookingSystem.Configurations.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            BadmintonCenterProfile();
        }
        private void BadmintonCenterProfile() {
            CreateMap<BadmintonCenterEntity, ResponseBadmintonCenterDTO>().ReverseMap();
            CreateMap<BadmintonCenterEntity, BadmintonCenterCreateDTO>().ReverseMap();
            CreateMap<BadmintonCenterEntity, BadmintonUpdateDTO>().ReverseMap();
        }
    }
}
