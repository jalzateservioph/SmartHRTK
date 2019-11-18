using IntegrationClient.DAL.Interfaces;
using IntegrationClient.DAL.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zkemkeeper;

namespace IntegrationClient.DAL.Services
{
    public class ZKTEcoDeviceService : IDeviceService
    {
        private IConfiguration _configuration;
        private ILoggingService _loggingService;
        private CZKEMClass device;
        private readonly int machineNumber = 1; //Not sure why this is 1 but this is what is set on the SDK.

        public ZKTEcoDeviceService(IConfiguration configuration, ILoggingService loggingService)
        {
            _configuration = configuration;
            _loggingService = loggingService;
            device = new CZKEMClass();
        }

        public IEnumerable<RawData> GetRawData(DateTime? from, DateTime? to)
        {
            ConnectToDevice();
            //Disable the device
            EnableDevice(false);
            //Get records
            List<RawData> rawData = new List<RawData>();
            _loggingService.Log("Reading raw data from device...", Enums.LogLevel.Info);
            string sdwEnrollNumber = "";
            int idwVerifyMode = 0;
            int idwInOutMode = 0;
            int idwYear = 0;
            int idwMonth = 0;
            int idwDay = 0;
            int idwHour = 0;
            int idwMinute = 0;
            int idwSecond = 0;
            int idwWorkcode = 0;

            if (from.HasValue && to.HasValue) //Filter by date
            {
                if (device.ReadTimeGLogData(machineNumber, from.Value.ToString("yyyy-MM-dd HH:mm:ss"), to.Value.ToString("yyyy-MM-dd HH:mm:ss")))
                {
                    while (device.SSR_GetGeneralLogData(machineNumber, out sdwEnrollNumber, out idwVerifyMode, out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))
                    {
                        RawData raw = new RawData()
                        {
                            EmployeeBiometricsID = sdwEnrollNumber,
                            TransactionDateTime = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond)
                        };

                        rawData.Add(raw);
                    }
                }
            }
            else
            {
                if (device.ReadGeneralLogData(machineNumber))
                {


                    while (device.SSR_GetGeneralLogData(machineNumber, out sdwEnrollNumber, out idwVerifyMode, out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))
                    {
                        RawData raw = new RawData()
                        {
                            EmployeeBiometricsID = sdwEnrollNumber,
                            TransactionDateTime = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond)
                        };

                        rawData.Add(raw);
                    }
                }
            }
            
            
            //Enable the device
            EnableDevice(true);
            DisconnectFromDevice();
            return rawData;        
        }

        public void EnrollUsers(IEnumerable<Employee> employees) 
        {
            if (employees != null)
            {
                _loggingService.Log($"Employees count: {employees.Count()}", Enums.LogLevel.Info);
                foreach (var employee in employees)
                {
                    ConnectToDevice();
                    EnableDevice(false);
                    device.SetStrCardNumber("");
                    if (device.SSR_SetUserInfo(machineNumber, employee.BiometricsId, employee.EmployeeName, "123", 0, true))
                    {
                        _loggingService.Log($"Successfully created/updated user {employee.BiometricsId} - {employee.EmployeeName}", Enums.LogLevel.Info);
                    }
                    else
                    {
                        _loggingService.Log($"Failed creating/updating user {employee.BiometricsId} - {employee.EmployeeName}", Enums.LogLevel.Warn);
                    }
                    EnableDevice(true);
                }

                DisconnectFromDevice();
            }

        }

        private void ConnectToDevice()
        {
            string config = _configuration.GetConnectionString("ZKTEcoDevice");

            string[] _config = config.Split(';');
            bool invalid = false;
            string IP = "";
            int port = 0;
            int commKey = 0;

            if (_config.Length != 3)
            {
                invalid = true;
            }
            else
            {
                IP = _config[0];

                if (!int.TryParse(_config[1], out port))
                {
                    invalid = true;
                }
                if (!int.TryParse(_config[2], out commKey))
                {
                    invalid = true;
                }
            }


            if (invalid)
            {
                _loggingService.Log("Invalid device connection string", Enums.LogLevel.Fatal);
                throw new Exception("Invalid device connection string");
            }
            device.SetCommPassword(commKey);


            _loggingService.Log("Connecting to Device...", Enums.LogLevel.Info);
            if (device.Connect_Net(IP, port))
            { 
                _loggingService.Log("Connection successful", Enums.LogLevel.Info);
            }
            else
            {
                _loggingService.Log("Connection failed", Enums.LogLevel.Fatal);
                throw new Exception("Connection to device failed");
            }
        }

        private void DisconnectFromDevice()
        {
            device.Disconnect();
        }


        private void EnableDevice(bool enable)
        {
            device.EnableDevice(machineNumber, enable);
        }

    }
}
