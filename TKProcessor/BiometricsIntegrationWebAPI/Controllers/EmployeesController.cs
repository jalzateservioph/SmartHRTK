using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BiometricsIntegrationWebAPI;
using BiometricsIntegrationWebAPI.Models;
using BiometricsIntegrationWebAPI.Services;
using AutoMapper;
using Integration = IntegrationClient.DAL.Models;
using TK = TKProcessor.Models.TK;
namespace BiometricsIntegrationWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public ActionResult<IEnumerable<Employee>> GetEmployees()
        {
            var employees = new List<Integration.Employee>();
            foreach (var e in service.GetEmployees())
            {
                employees.Add(mapper.Map<TK.Employee, Integration.Employee>(e));
            }
            return new JsonResult(employees);
        }

    }
}
