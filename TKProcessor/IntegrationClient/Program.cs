using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using IntegrationClient.DAL.Interfaces;
using IntegrationClient.DAL.Services;
using System.Collections.Generic;
using IntegrationClient.DAL.Models;
using System.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace IntegrationClient
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        private static IConfigurationRoot _configuration;

        private static LastRun lastRun;
        private static string fileName = "lastsuccessfulrun.json";
        private static string path = GetBasePath() + "/" + fileName;
        private static bool updateLastRun = false;

        static void Main(string[] args)
        {
            BuildConfiguration();
            RegisterServices();

            ILoggingService loggingService = _serviceProvider.GetService<ILoggingService>();
            IDeviceService deviceService = _serviceProvider.GetRequiredService<IDeviceService>();
            ITimekeepingService tkService = _serviceProvider.GetRequiredService<ITimekeepingService>();

            try
            {
                ReadLastSuccessfulRun();
                if (args.Length == 0) //Invoked by user
                {
                    Console.WriteLine("TIMEKEEPING BIOMETRICS INTEGRATION MODULE");

                Start:
                    Console.WriteLine("Select operation:\n1. Pull employee data from TK\n2. Push raw data to TK");
                    string input = Console.ReadLine();

                    int parsedInput = 0;

                    if (int.TryParse(input, out parsedInput))
                    {
                        if (parsedInput == 1) //Pull employee data
                        {
                            //Connect to WebAPI
                            try
                            {
                                var employees = tkService.GetEmployees();
                                deviceService.EnrollUsers(employees);
                                lastRun.lastSuccessfulPull = DateTime.Now;
                                updateLastRun = true;
                            }
                            catch (Exception ex)
                            {
                                loggingService.Log(ex.Message, DAL.Enums.LogLevel.Fatal);
                            }
                        }
                        else if (parsedInput == 2) //Push raw data
                        {
                        Start2:
                            Console.WriteLine("Please input date range. First date value is from and second is to. Separate values using comma(,). If no range is supplied, only records from the last successful run to the current day will be processed\nFormat: yyyy-MM-dd");
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
                                    tkService.PushRawData(rawData);
                                    lastRun.lastSuccessfulPush = to;
                                    updateLastRun = true;
                                }
                                catch (Exception ex)
                                {
                                    loggingService.Log(ex.Message, DAL.Enums.LogLevel.Fatal);
                                }

                            }
                            else if (String.IsNullOrWhiteSpace(input))
                            {
                                try
                                {
                                    IEnumerable<RawData> rawData = lastRun.lastSuccessfulPush.HasValue ? deviceService.GetRawData(lastRun.lastSuccessfulPush, DateTime.Now) : deviceService.GetRawData(null, null); //from last successful push to current date
                                    tkService.PushRawData(rawData);
                                    lastRun.lastSuccessfulPush = DateTime.Now;
                                    updateLastRun = true;
                                }
                                catch (Exception ex)
                                {
                                    loggingService.Log(ex.Message, DAL.Enums.LogLevel.Fatal);
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

                    Console.ReadLine();

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
                     *      optional arguments for date filters
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
                        try
                        {
                            var employees = tkService.GetEmployees();
                            deviceService.EnrollUsers(employees);
                        }
                        catch (Exception ex)
                        {
                            loggingService.Log(ex.Message, DAL.Enums.LogLevel.Fatal);
                        }

                        lastRun.lastSuccessfulPull = DateTime.Now;
                        updateLastRun = true;
                    }
                    else if (args[0].ToLower().Equals("push-rawdata"))
                    {
                        //arg format for date range should be from=yyyy-MM-dd to=yyyy-MM-dd
                        if (args.Length == 1) //No date range
                        {
                            IEnumerable<RawData> rawData = lastRun.lastSuccessfulPush.HasValue ? deviceService.GetRawData(lastRun.lastSuccessfulPush, DateTime.Now) : deviceService.GetRawData(null, null); //from last successful push to current date
                            tkService.PushRawData(rawData);
                            lastRun.lastSuccessfulPush = DateTime.Now;
                            updateLastRun = true;
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
                                        tkService.PushRawData(rawData);
                                        //push data to WebAPI
                                    }
                                    catch (Exception ex)
                                    {
                                        loggingService.Log(ex.Message, DAL.Enums.LogLevel.Fatal);
                                    }
                                    lastRun.lastSuccessfulPush = DateTime.Now;
                                    updateLastRun = true;
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
            }
            catch (Exception ex)
            {
                loggingService.Log(ex.StackTrace, DAL.Enums.LogLevel.Fatal);

                throw ex;
            }

            if (updateLastRun) WriteLastSuccessfulRun();
            DisposeServices();
        }

        private static void BuildConfiguration()
        {
            //Console.WriteLine("Base Path: " + GetBasePath());
            var builder = new ConfigurationBuilder().SetBasePath(GetBasePath()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();
        }

        private static string GetBasePath()
        {
            string assemblyPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            //Console.WriteLine("Assembly Path: " + assemblyPath);
            //Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");

            //return appPathMatcher.Match(assemblyPath).Value;
            return assemblyPath.Replace("file:\\", null);
        }
        private static void RegisterServices()

        {
            var collection = new ServiceCollection();

            collection.AddSingleton<IConfiguration>(_ => _configuration);
            collection.AddSingleton<ILoggingService, NLogService>();
            collection.AddSingleton<ITimekeepingService, WebAPIService>();
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

        private static void WriteLastSuccessfulRun()
        {
            string json = JsonConvert.SerializeObject(lastRun);
            File.WriteAllText(path, json);
        }

        private static void ReadLastSuccessfulRun()
        {
            if (File.Exists(path))
            {
                string file = File.ReadAllText(path);
                lastRun = JsonConvert.DeserializeObject<LastRun>(file);
            }
            else
            {
                lastRun = new LastRun();
                updateLastRun = true;
            }
        }

        class LastRun
        {
            public DateTime? lastSuccessfulPush { get; set; }
            public DateTime? lastSuccessfulPull { get; set; }
        }


    }

}

