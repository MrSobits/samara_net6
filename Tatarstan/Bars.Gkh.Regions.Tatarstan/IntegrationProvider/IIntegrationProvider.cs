namespace Bars.Gkh.Regions.Tatarstan.IntegrationProvider
{
    using Castle.Windsor;

    public interface IIntegrationProvider<T> 
        where T : class
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
}