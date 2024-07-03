using AutoMapper;
using BadmintonBookingSystem.BusinessObject.DTOs.RequestDTOs;
using BadmintonBookingSystem.BusinessObject.DTOs.ResponseDTOs;
using BadmintonBookingSystem.DataAccessLayer.Entities;
using System.Data;

namespace BadmintonBookingSystem.Configurations.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            BadmintonCenterProfile();
            UserMappingProfile();
        }
        private void BadmintonCenterProfile() {
            CreateMap<BadmintonCenterEntity, ResponseBadmintonCenterDTO>().ReverseMap();
            CreateMap<BadmintonCenterEntity, BadmintonCenterCreateDTO>().ReverseMap();
            CreateMap<BadmintonCenterEntity, BadmintonUpdateDTO>().ReverseMap();
        }
        private void UserMappingProfile()
        {
            CreateMap<UserEntity, RegisterDTO>().ReverseMap();
            CreateMap<UserEntity, ResponseUserDTO>().ReverseMap();
        }
    }
}
