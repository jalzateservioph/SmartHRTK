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
        }
    }
}
