namespace Bars.Gkh.Reforma.Interface
{
    using Bars.Gkh.Reforma.Interface.Logger;
    using Bars.Gkh.Reforma.Interface.Performer;
    using Bars.Gkh.Reforma.Interface.Session;
    using Bars.Gkh.Reforma.ReformaService;

    /// <summary>
    /// Провайдер синхронизации с Реформой ЖКХ
    /// </summary>
    public interface ISyncProvider
    {
        /// <summary>
        /// Клиент сервиса Реформы ЖКХ
        /// </summary>
        ApiSoapPortClient Client { get; }

        /// <summary>
        /// Планировщик действий синхронизации
        /// </summary>
        ISyncActionPerformer Performer { get; }

        /// <summary>
        /// Сессия синхронизации
        /// </summary>
        ISyncSession Session { get; }

        /// <summary>
        /// Логгер действий
        /// </summary>
        ISyncLogger Logger { get; }

        /// <summary>
        /// Завершение действий
        /// </summary>
        void Close();
    }
}