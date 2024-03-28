namespace Bars.GkhEdoInteg.Configuration
{
    using Bars.B4.Config;

    using Castle.Windsor;

    /// <summary>
    /// Класс содержащий конфигурацию модуля Bars.GkhEdoInteg
    /// </summary>
    internal static class EdoModuleConfiguration
    {
        /// <summary>
        /// Версия API
        /// </summary>
        public static string ApiVersion { get; private set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public static string UserId { get; private set;  }

        /// <summary>
        /// Хэшированый пароль (Base64)
        /// </summary>
        public static string PasswordHash { get; private set;  }

        /// <summary>
        /// Таймаут запросов
        /// </summary>
        public static int RequestTimeout { get; private set;  }

        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="container"><see cref="IWindsorContainer"/></param>
        public static void Init(IWindsorContainer container)
        {
            var moduleParameters = container.Resolve<IConfigProvider>().GetConfig().GetModuleConfig("Bars.GkhEdoInteg");
            
            ApiVersion = moduleParameters.GetAs("apiVersion", "1");
            UserId = moduleParameters.GetAs<string>("userId");
            PasswordHash = moduleParameters.GetAs<string>("passwordHash");
            RequestTimeout = moduleParameters.GetAs("requestTimeout", 300000);
        }
    }
}