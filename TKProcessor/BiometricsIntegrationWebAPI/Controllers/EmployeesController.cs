using AutoMapper;
using BiometricsIntegrationWebAPI.Models;
using BiometricsIntegrationWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Integration = IntegrationClient.DAL.Models;
using TK = TKProcessor.Models.TK;

namespace BiometricsIntegrationWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeService service;
        private readonly IMapper mapper;

        public EmployeesController(EmployeeService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        // GET: api/Employees
        [HttpGet]
        public ActionResult<IEnumerable<Integration.Employee>> GetEmployees()
        {
            string siteId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var employees = new List<Integration.Employee>();
            var q = service.GetEmployees(Guid.Parse(siteId));
            foreach (var e in service.GetEmployees(Guid.Parse(siteId)))
            {
                employees.Add(mapper.Map<TK.Employee, Integration.Employee>(e));
            }
            return new JsonResult(employees);
        }

    }
}
