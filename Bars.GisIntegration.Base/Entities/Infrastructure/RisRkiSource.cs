namespace Bars.GisIntegration.Base.Entities.Infrastructure
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Источник ОКИ
    /// </summary>
    public class RisRkiSource : BaseRisEntity
    {
        /// <summary>
        /// Объект коммунальной инфраструктуры
        /// </summary>
        public virtual RisRkiItem RkiItem { get; set; }

        /// <summary>
        /// ОКИ, который является источником для RkiItem
        /// </summary>
        public virtual RisRkiItem SourceRkiItem { get; set; }
    }
}