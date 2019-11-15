using System;
using System.Collections.Generic;
using System.Text;
using IntegrationClient.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using NLog;
using LogLevel = IntegrationClient.DAL.Enums.LogLevel;

namespace IntegrationClient.DAL.Services
{
    public class NLogService : ILoggingService
    {
        private readonly IConfiguration _configuration;
        private Logger logger;

        public NLogService(IConfiguration configuration)
        {
            _configuration = configuration;
            InitializeLogger();
        }

        private void InitializeLogger()
        {
            //logger = NLog.LogManager.GetCurrentClassLogger();

            var config = new NLog.Config.LoggingConfiguration();

            var logFile = new NLog.Targets.FileTarget("logFile") { FileName = "${basedir}/logs/${shortdate}.log" };
            var logConsole = new NLog.Targets.ConsoleTarget("logconsole");
            logConsole.Layout = new NLog.Layouts.SimpleLayout("${uppercase:${level}}: ${message}");

            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logConsole);
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, logFile);

            NLog.LogManager.Configuration = config;

            logger = NLog.LogManager.GetCurrentClassLogger();
        }
        public void Log(string s, LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Trace:
                    logger.Trace(s);
                    break;
                case LogLevel.Debug:
                    logger.Debug(s);
                    break;
                case LogLevel.Info:
                    logger.Info(s);
                    break;
                case LogLevel.Warn:
                    logger.Warn(s);
                    break;
                case LogLevel.Error:
                    logger.Error(s);
                    break;
                case LogLevel.Fatal:
                    logger.Fatal(s);
                    break;
            }
        }
    }
}
