using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using IntegrationClient.DAL.Interfaces;
using IntegrationClient.DAL.Services;
using System.Collections.Generic;
using IntegrationClient.DAL.Models;
using System.Linq;
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
            IDeviceService deviceService = _serviceProvider.GetRequiredService<IDeviceService>();

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
                        //Connect to WebAPI
                        deviceService.EnrollUsers(null);

                    }
                    else if (parsedInput == 2) //Push raw data
                    {
                        Start2:
                        Console.WriteLine("Please input date range. First date value is from and second is to. Separate values using comma(,). If no range is supplied all records will be pushed to TK, \nFormat: yyyy-MM-dd");
                        input = Console.ReadLine();

                        string[] _input = input.Split(',');
                        if (_input.Length == 2)
                        {
                            DateTime from;
                            DateTime to;

                            if (!DateTime.TryParse(_input[0], out from) || !DateTime.TryParse(_input[1], out to))
                            {
                                Console.Clear();
                                Console.WriteLine("Invalid input");
                                goto Start2;
                            }

                            if (to < from)
                            {
                                Console.Clear();
                                Console.WriteLine("Date range end should not be less than its start");
                                goto Start2;
                            }
                            try
                            {
                                IEnumerable<RawData> rawData = deviceService.GetRawData(from, to);

                                //push data to WebAPI
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else if (String.IsNullOrWhiteSpace(input))
                        {
                            try
                            {
                                IEnumerable<RawData> rawData = deviceService.GetRawData(null, null);
                                //push data to WebAPI
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Invalid input");
                            goto Start2;
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
                if (args[0].ToLower().Equals("pull-employeedata"))
                {
                    //Connect to WebAPI
                    deviceService.EnrollUsers(null);
                }
                else if (args[0].ToLower().Equals("push-rawdata"))
                {
                    //arg format for date range should be from=yyyy-MM-dd to=yyyy-MM-dd
                    if (args.Length == 1) //No date range
                    {
                        IEnumerable<RawData> rawData = deviceService.GetRawData(null, null);
                        //push data to WebAPI
                    }
                    else
                    {
                        string dateFrom = args.Where(s => s.ToLower().StartsWith("from")).FirstOrDefault();
                        string dateTo = args.Where(s => s.ToLower().StartsWith("to")).FirstOrDefault();

                        if (String.IsNullOrWhiteSpace(dateFrom) || String.IsNullOrWhiteSpace(dateTo))
                        {
                            loggingService.Log("Invalid arguments", DAL.Enums.LogLevel.Fatal);
                        }
                        else
                        {
                            string[] _dateFrom = dateFrom.Split('=');
                            string[] _dateTo = dateTo.Split('=');

                            if (_dateFrom.Length == 2 && _dateTo.Length == 2)
                            {
                                DateTime from;
                                DateTime to;

                                if (!DateTime.TryParse(_dateFrom[1], out from) || !DateTime.TryParse(_dateTo[1], out to))
                                {
                                    loggingService.Log("Invalid arguments", DAL.Enums.LogLevel.Fatal);
                                    return;
                                }

                                if (to < from)
                                {
                                    loggingService.Log("Invalid arguments", DAL.Enums.LogLevel.Fatal);
                                    return;
                                }
                                try
                                {
                                    IEnumerable<RawData> rawData = deviceService.GetRawData(from, to);

                                    //push data to WebAPI
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            else
                            { 
                                loggingService.Log("Invalid arguments", DAL.Enums.LogLevel.Fatal);
                            }
                        }


                    }
                }
                else 
                {
                    loggingService.Log("Invalid arguments", DAL.Enums.LogLevel.Fatal);
                }
            }
            DisposeServices();
         }

        private static void BuildConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();
        }

        private static void RegisterServices()
        {
            var collection = new ServiceCollection();

            collection.AddSingleton<IConfiguration>(_ => _configuration);
            collection.AddSingleton<ILoggingService,NLogService>();
            collection.AddSingleton<IEmployeeService, WebAPIEmployeeService>();
            collection.AddSingleton<IDeviceService, ZKTEcoDeviceService>(); //Change this depending on the device

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

