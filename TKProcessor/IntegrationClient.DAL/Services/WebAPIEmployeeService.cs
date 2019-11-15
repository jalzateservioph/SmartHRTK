using System;
using System.Collections.Generic;
using System.Text;
using IntegrationClient.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using IntegrationClient.DAL.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IntegrationClient.DAL.Services
{
    public class WebAPIEmployeeService : IEmployeeService
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggingService _loggingService;
        private HttpClient httpClient;
        public WebAPIEmployeeService(IConfiguration configuration, ILoggingService loggingService)
        {
            _configuration = configuration;
            _loggingService = loggingService;
            InitializeHttpClient();
        }

        private void InitializeHttpClient()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_configuration.GetConnectionString("TK_WebAPI"));
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IEnumerable<Employee> GetEmployees()
        {

            return null; //To do
        }
    }
}
