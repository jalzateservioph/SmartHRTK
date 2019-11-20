using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BiometricsIntegrationWebAPI;
using Microsoft.AspNetCore.Authorization;
using BiometricsIntegrationWebAPI.Services;
using IntegrationClient.DAL.Models;
namespace BiometricsIntegrationWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RawDataController : ControllerBase
    {
        private readonly RawDataService service;

        public RawDataController(RawDataService service)
        {
            this.service = service;
        }

        [HttpPost]
        public ActionResult PostRawData(IEnumerable<RawData> rawData)
        {
            try
            {
                service.PushRawData(rawData);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
            return new OkResult();
        }


    }
}

