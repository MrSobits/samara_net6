namespace Bars.GisIntegration.Smev.EventHandlers
{
    using Bars.B4.Application;
    using Bars.B4.Config;
    using Bars.B4.Events;
    using Bars.B4.Utils;
    using Bars.Gkh.Gis.RabbitMQ;
    using Bars.Gkh.Smev3;

    using Castle.Windsor;

    /// <summary>
    /// Обработчик события инициализации приложения. Подписка на очередь сообщений с ответами из шлюза СМЭВ
    /// </summary>
    public class QueueSubscriber : EventHandlerBase<AppStartEventArgs>
    {
        private IWindsorContainer container => ApplicationContext.Current.Container;
        
        /// <inheritdoc />
        public override void OnEvent(AppStartEventArgs args)
        {
            var appSettings = this.container.Resolve<IConfigProvider>().GetConfig().GetModuleConfig("Bars.Gkh.Gis");

            if (appSettings != null && appSettings[SettingsKeyStore.Enable].To<bool>())
            {
                var rabbitConsumerService = this.container.Resolve<IConsumerService>();
                
                rabbitConsumerService.StartConsuming<Smev3Response>("smev3.callback");
            }
        }
    }
}