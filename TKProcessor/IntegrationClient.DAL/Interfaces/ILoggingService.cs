using System;
using System.Collections.Generic;
using System.Text;
using IntegrationClient.DAL.Enums;
namespace IntegrationClient.DAL.Interfaces
{
    public interface ILoggingService
    {
        void Log(string s, LogLevel level);
    }
}
