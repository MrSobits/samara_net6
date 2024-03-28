namespace Bars.GisIntegration.Base.GisServiceProvider
{
    using Bars.GisIntegration.Base.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Интерфейс поставщика объектов для работы с сервисом ГИС с учетом хранимых настроек
    /// </summary>
    /// <typeparam name="T">Тип soap-клиента</typeparam>
    public interface IGisServiceProvider<T> : IGisServiceProvider
    {
        /// <summary>
        /// Ioc контейнер
        /// </summary>
        IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить soap клиент для работы с сервисом
        /// </summary>
        T GetSoapClient();
    }

    /// <summary>
    /// Интерфейс поставщика объектов для работы с сервисом ГИС с учетом хранимых настроек
    /// </summary>
    public interface IGisServiceProvider
    {
        /// <summary>
        /// Значение перечисления, соответствующее сервису
        /// </summary>
        IntegrationService IntegrationService { get; }

        /// <summary>
        /// Адрес сервиса
        /// </summary>
        string ServiceAddress { get; }
    }
}