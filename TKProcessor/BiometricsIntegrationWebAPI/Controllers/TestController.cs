using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TK = TKProcessor.Contexts;
using TKModel = TKProcessor.Models.TK;
namespace BiometricsIntegrationWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly TK.TKContext context;

        public TestController(TK.TKContext context)
        {
            this.context = context;
        }

        public ActionResult<TKModel.Employee> Get()
        {
            return new JsonResult(context.Employee.ToList());
        }

    }
}
