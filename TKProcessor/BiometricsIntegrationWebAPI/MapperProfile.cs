using AutoMapper;
using TK = TKProcessor.Models.TK;
using Integration = IntegrationClient.DAL.Models;
namespace BiometricsIntegrationWebAPI
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<TK.Employee, Integration.Employee>().ForMember(dest => dest.EmployeeName, act => act.MapFrom(src => src.FullName)).ReverseMap();
            CreateMap<Integration.RawData, TK.RawData>()
                .ForMember(dest => dest.BiometricsId, act => act.MapFrom(src => src.EmployeeBiometricsID))
                .ForMember(dest => dest.TransactionType, act => act.MapFrom(src => src.TransactionType))
                .ForMember(dest => dest.TransactionDateTime, act => act.MapFrom(src => src.TransactionDateTime))
                .ReverseMap();
        }
    }
}
