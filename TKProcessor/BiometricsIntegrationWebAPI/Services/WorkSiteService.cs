using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKProcessor.Models.TK;
using TKProcessor.Contexts;
namespace BiometricsIntegrationWebAPI.Services
{
    public class WorkSiteService
    {
        private readonly TKContext context;

        public WorkSiteService(TKContext context)
        {
            this.context = context;
        }
        public async Task<WorkSite> Authenticate(string username, string password)
        {
            var workSite = await Task.Run(() => context.WorkSite.SingleOrDefault(x => x.IntegrationAuthUsername == username && x.IntegrationAuthPassword == password)); //Change for hashing

            return workSite;
        }
    }
}
