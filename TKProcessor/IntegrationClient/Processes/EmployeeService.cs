using IntegrationClient.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationClient.Processes
{
    public class EmployeeService
    {
        public async Task<IEnumerable<Employee>> GetProducts()
        {
            using HttpClient client = new HttpClient() { BaseAddress = new Uri("http://localhost:54741/") };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/Employees");

            response.EnsureSuccessStatusCode();

            List<Employee> resultSet = null;

            if (response.IsSuccessStatusCode)
            {
                resultSet = await response.Content.ReadAsAsync<List<Employee>>();
            }

            return resultSet;
        }
    }
}
