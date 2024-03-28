namespace Bars.Esia.OAuth20.App.Application
{
    using System;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.Logging.LogManagers;

    /// <summary>
    /// Менеджер логирования сервиса авторизации
    /// </summary>
    public class AuthAppLogManager : BaseLogManager
    {
        public AuthAppLogManager()
        {
            if (this.GetConfigFile("b4.user.config").Exists)
            {
                this.Initialize("b4.user.config");
            }
            else
            {
                this.Initialize("b4.config");
            }
        }

        /// <inheritdoc />
        public override void Info(string message)
        {
            foreach (var loggerInfo in this.LogList.Values)
            {
                loggerInfo.Log.Info(message);
                this.AdditionalMessageSending(message);
            }
        }

        /// <inheritdoc />
        public override void Error(string message)
        {
            foreach (var loggerInfo in this.LogList.Values.Where(x => x.IsActive))
            {
                loggerInfo.Log.Error(message);
                this.AdditionalMessageSending(message);
            }
        }

        /// <summary>
        /// Дополнительная отправка сообщения
        /// </summary>
        private void AdditionalMessageSending(string message)
        {
            Console.WriteLine($"{DateTime.Now} ### {message}\n");
        }
    }
}