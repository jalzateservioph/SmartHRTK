using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using IntegrationClient.DAL.Interfaces;
using IntegrationClient.DAL.Services;
namespace IntegrationClient
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        private static IConfigurationRoot _configuration;

        static void Main(string[] args)
        {
            BuildConfiguration();
            RegisterServices();

            ILoggingService loggingService = _serviceProvider.GetService<ILoggingService>();
            //Test connection to both TK and Device
            


            if (args.Length == 0) //Invoked by user
            {
                Start:
                Console.WriteLine("Select operation:\n1. Pull employee data from TK\n2. Push raw data to TK");
                string input = Console.ReadLine();

                int parsedInput = 0;

                if (int.TryParse(input, out parsedInput))
                {
                    if (parsedInput == 1) //Pull employee data
                    {

                    }
                    else if (parsedInput == 2) //Push raw data
                    {

                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid input");
                        goto Start;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input");
                    goto Start;
                }

            }
            else 
            {
                /*
                 * Read args
                 * Look for the following commands: push-rawdata, pull-employeedata
                 * 
                 * 
                 * 
                 * push-rawdata:
                 *      need to provide additional arguments for date filters
                 *      invoke Biometrics service, get rawdata qualified by said filters
                 *      invoke API service, push rawdata, include any additional data for client identification
                 *      
                 * pull-employedata:
                 *      invoke API service, get employee data
                 *      
                 *      
                 */
            }

            DisposeServices();
         }

        private static void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _configuration = builder.Build();
        }

        private static void RegisterServices()
        {
            var collection = new ServiceCollection();

            collection.AddScoped<IConfiguration>(_ => _configuration);
            collection.AddSingleton<ILoggingService,NLogService>();
            collection.AddSingleton<IEmployeeService, WebAPIEmployeeService>();

            //add services here

            _serviceProvider = collection.BuildServiceProvider();
        }

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }

    }
}

