using IntegrationClient.DAL.Interfaces;
using IntegrationClient.DAL.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zkemkeeper;
using TK = TKProcessor.Models.TK;
namespace IntegrationClient.DAL.Services
{
    public class ZKTEcoDeviceService : IDeviceService
    {
        private IConfiguration _configuration;
        private ILoggingService _loggingService;
        private List<CZKEMClass> deviceList;
        private readonly int machineNumber = 1; //Not sure why this is 1 but this is what is set on the SDK.

        public ZKTEcoDeviceService(IConfiguration configuration, ILoggingService loggingService)
        {
            _configuration = configuration;
            _loggingService = loggingService;
            deviceList = new List<CZKEMClass>();
        }

        public IEnumerable<RawData> GetRawData(DateTime? from, DateTime? to)
        {
            if(from.HasValue && to.HasValue) _loggingService.Log($"Fetching raw data from {from.Value.ToString("yyyy-MM-dd")} to {to.Value.ToString("yyyy-MM-dd")}", Enums.LogLevel.Info);
            else _loggingService.Log("Fetching ALL raw data", Enums.LogLevel.Info);
            ConnectToDevices();
            List<RawData> rawData = new List<RawData>();
            foreach (var device in deviceList)
            {
                EnableDevice(device,false);

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

                EnableDevice(device, true);


                if (from.HasValue && to.HasValue) //Filter by date
                {
                    if (device.ReadTimeGLogData(machineNumber, from.Value.ToString("yyyy-MM-dd HH:mm:ss"), to.Value.ToString("yyyy-MM-dd HH:mm:ss")))
                    {
                        while (device.SSR_GetGeneralLogData(machineNumber, out sdwEnrollNumber, out idwVerifyMode, out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))
                        {
                            int transactionType = 0;
                            switch (idwInOutMode) 
                            {
                                case 0:
                                    transactionType = (int)TK.TransactionType.TimeIn;
                                    break;
                                case 1:
                                    transactionType = (int)TK.TransactionType.TimeOut;
                                    break;
                                case 2: //Overtime in
                                    //transactionType = (int)TransactionType.TimeIn;
                                    break;
                                case 3: //Overtime out
                                    //transactionType = (int)TransactionType.TimeIn;
                                    break;
                                case 4: //Break out
                                    //transactionType = (int)TransactionType.Break;
                                    break;
                                case 5: //Break in
                                    //transactionType = (int)TransactionType.TimeIn;
                                    break;
                            };
                            RawData raw = new RawData()
                            {
                                EmployeeBiometricsID = sdwEnrollNumber,
                                TransactionDateTime = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond),
                                TransactionType = transactionType
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
                            int transactionType = 0;
                            switch (idwInOutMode)
                            {
                                case 0:
                                    transactionType = (int)TK.TransactionType.TimeIn;
                                    break;
                                case 1:
                                    transactionType = (int)TK.TransactionType.TimeOut;
                                    break;
                                case 2: //Overtime in
                                    //transactionType = (int)TransactionType.TimeIn;
                                    break;
                                case 3: //Overtime out
                                    //transactionType = (int)TransactionType.TimeIn;
                                    break;
                                case 4: //Break out
                                    //transactionType = (int)TransactionType.Break;
                                    break;
                                case 5: //Break in
                                    //transactionType = (int)TransactionType.TimeIn;
                                    break;
                            };
                            RawData raw = new RawData()
                            {
                                EmployeeBiometricsID = sdwEnrollNumber,
                                TransactionDateTime = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond),
                                TransactionType = transactionType
                            };

                            rawData.Add(raw);
                        }
                    }
                }
            }
            DisconnectFromDevices();
            return rawData;        
        }

        public void EnrollUsers(IEnumerable<Employee> employees) 
        {
            _loggingService.Log($"Employees count from TK: {employees.Count()}", Enums.LogLevel.Info);
            ConnectToDevices();
            foreach (var device in deviceList)
            {
                //IList<Employee> empList = (IList<Employee>)employees;
                IList<Employee> empList = employees.ToList();
                EnableDevice(device, false);
                device.ReadAllUserID(machineNumber);

                string sEnrollNumber = "";
                string sName = "";
                string sPassword = "";
                int iPrivilege = 0;
                bool bEnabled = false;

                while (device.SSR_GetAllUserInfo(machineNumber, out sEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))
                {
                    int index = empList.Select(e => e.BiometricsId).ToList().IndexOf(sEnrollNumber); // enrollNumber matches any from empList, returns -1 if not found in list
                    if (index >= 0)
                    {
                        empList.RemoveAt(index);
                    }
                }

                foreach (var employee in empList)
                {
                    device.SetStrCardNumber("");
                    if (device.SSR_SetUserInfo(machineNumber, employee.BiometricsId, employee.EmployeeName, "123", 0, true))
                    {
                        _loggingService.Log($"Successfully created/updated user {employee.BiometricsId} - {employee.EmployeeName}", Enums.LogLevel.Info);
                    }
                    else
                    {
                        _loggingService.Log($"Failed creating/updating user {employee.BiometricsId} - {employee.EmployeeName}", Enums.LogLevel.Warn);
                    }
                }

                EnableDevice(device, true);
            }
            DisconnectFromDevices();
        }

        private void ConnectToDevices()
        {
            IEnumerable<DeviceConnectionString> conStringList = _configuration.GetSection("ZKTEcoDevice").Get<DeviceConnectionString[]>();

            foreach(var conString in conStringList)
            {
                CZKEMClass device = new CZKEMClass();
                device.SetCommPassword(int.Parse(conString.CommKey));

                _loggingService.Log($"Connecting to Device {conString.Name}...", Enums.LogLevel.Info);
                if (device.Connect_Net(conString.IP, int.Parse(conString.Port)))
                {
                    _loggingService.Log($"Connection to {conString.Name} successful", Enums.LogLevel.Info);
                    deviceList.Add(device);
                }
                else
                {
                    _loggingService.Log($"Connection to {conString.Name} failed", Enums.LogLevel.Fatal);
                    throw new Exception($"Connection to {conString.Name} failed");
                }

            }
        }

        private void DisconnectFromDevices()
        {
            foreach(var device in deviceList)
            {
                device.Disconnect();
            }
        }


        private void EnableDevice(CZKEMClass device, bool enable)
        {
            device.EnableDevice(machineNumber, enable);
        }



        class DeviceConnectionString
        {
            public string Name { get; set; }
            public string IP { get; set; }
            public string Port { get; set; }
            public string CommKey { get; set; }
        }
    }
}
