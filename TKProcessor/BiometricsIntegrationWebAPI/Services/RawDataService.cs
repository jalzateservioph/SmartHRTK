using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integration = IntegrationClient.DAL.Models;
using Context = TKProcessor.Contexts;
using TK = TKProcessor.Models.TK;
using AutoMapper;

namespace BiometricsIntegrationWebAPI.Services
{
    public class RawDataService
    {
        private readonly Context.TKContext context;
        private readonly IMapper mapper;

        public RawDataService(Context.TKContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void PushRawData(IEnumerable<Integration.RawData> rawData)
        {
            foreach (var i in rawData)
            {
                context.RawData.Add(mapper.Map<Integration.RawData, TK.RawData>(i));
            }
            context.SaveChanges();
        }
    }
}
