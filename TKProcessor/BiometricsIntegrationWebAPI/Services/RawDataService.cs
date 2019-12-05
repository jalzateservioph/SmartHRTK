using AutoMapper;
using System.Collections.Generic;
using Context = TKProcessor.Contexts;
using Integration = IntegrationClient.DAL.Models;
using TK = TKProcessor.Models.TK;
using TKServices = TKProcessor.Services.Maintenance;
namespace BiometricsIntegrationWebAPI.Services
{
    public class RawDataService
    {
        private readonly TKServices.RawDataService rawDataService;
        private readonly IMapper mapper;

        public RawDataService(Context.TKContext context, IMapper mapper, TKAuthService authService)
        {
            //this.context = context;
            this.mapper = mapper;
            rawDataService = new TKServices.RawDataService(authService.GetUser().Id, context);
        }

        public void PushRawData(IEnumerable<Integration.RawData> rawData)
        {
            foreach (var i in rawData)
            {
                rawDataService.Save(mapper.Map<Integration.RawData, TK.RawData>(i));
            }
        }
    }
}
