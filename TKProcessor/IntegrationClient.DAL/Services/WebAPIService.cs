using System;
using System.Collections.Generic;
using System.Text;
using IntegrationClient.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using IntegrationClient.DAL.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace IntegrationClient.DAL.Services
{
    public class WebAPIService : ITimekeepingService
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggingService _loggingService;
        private HttpClient httpClient;
        public WebAPIService(IConfiguration configuration, ILoggingService loggingService)
        {
            _configuration = configuration;
            _loggingService = loggingService;
            InitializeHttpClient();
        }

        private void InitializeHttpClient()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_configuration.GetSection("TK_WebAPI")["BaseURL"]);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //add auth
            string username = _configuration.GetSection("TK_WebAPI")["AuthUsername"];
            string password = _configuration.GetSection("TK_WebAPI")["AuthPassword"];
            var auth = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));

            httpClient.DefaultRequestHeaders.Authorization = auth;
        }

        public IEnumerable<Employee> GetEmployees()
        {
            string endpoint = _configuration.GetSection("TK_WebAPI")["GetEmployeeEndpoint"];
            HttpResponseMessage response = httpClient.GetAsync(endpoint).Result;
            if (response.IsSuccessStatusCode)
            {
                _loggingService.Log("Get employees successful", Enums.LogLevel.Info);
                var jsonString = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<IEnumerable<Employee>>(jsonString);
            }
            throw new Exception($"Get employees failed\n{response.Content.ToString()}");
        }

        public void PushRawData(IEnumerable<RawData> rawData)
        {
            _loggingService.Log("Raw data count:", Enums.LogLevel.Info);
            string endpoint = _configuration.GetSection("TK_WebAPI")["PostRawDataEndpoint"];
            var jsonObject = JsonConvert.SerializeObject(rawData);
            var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
            HttpResponseMessage response = httpClient.PostAsync(endpoint, content).Result;

            if (response.IsSuccessStatusCode)
            {
                _loggingService.Log("Push successful", Enums.LogLevel.Info);
                return;
            }

            throw new Exception($"Push failed \n{response.Content.ToString()}");

        }
    }
}
