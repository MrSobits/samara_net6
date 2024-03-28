namespace Bars.GisIntegration.Base.Entities.Infrastructure
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Приемник ОКИ
    /// </summary>
    public class RisRkiReceiver : BaseRisEntity
    {
        /// <summary>
        /// Объект коммунальной инфраструктуры
        /// </summary>
        public virtual RisRkiItem RkiItem { get; set; }

        /// <summary>
        /// ОКИ, который является приемником для RkiItem
        /// </summary>
        public virtual RisRkiItem ReceiverRkiItem { get; set; }
    }
}