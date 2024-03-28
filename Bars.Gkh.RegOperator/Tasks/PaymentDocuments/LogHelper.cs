namespace Bars.Gkh.RegOperator.Tasks.PaymentDocuments
{
    using System.IO;
    using System.Linq;
    using B4.Application;

    using Bars.Gkh.Entities;

    using Entities;
    using NLog;
    using NLog.Config;
    using NLog.Targets;

    internal sealed class LogHelper
    {
        public LogHelper(IPeriod period)
        {
            InitNlog(period);
        }

        public Logger GetLogger()
        {
            return LogManager.GetLogger("report_log");
        }

        private void InitNlog(IPeriod period)
        {
            var config = LogManager.Configuration;
            if (config == null || config.AllTargets.Any(x => x.Name == "report_log"))
            {
                return;
            }

            var ftpPath = GetFtpPath();
            var logPath = Path.Combine(ftpPath, period.Name, ".logs");

            var target = new FileTarget()
            {
                Name = "report_log"
            };

            config.AddTarget("report_log", target);

            target.FileName = logPath + @"\${level}.log";
            target.Layout = "${date}: ${message}";
            target.ConcurrentWrites = true;

            var rule = new LoggingRule("report_log", LogLevel.Debug, target);

            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;
        }

        private string GetFtpPath()
        {
            var config = ApplicationContext.Current.Configuration;

            return config.AppSettings.GetAs<string>("FtpPath");
        }
    }
}