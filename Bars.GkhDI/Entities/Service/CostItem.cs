namespace Bars.GkhDi.Entities
{
    using Gkh.Entities;

    /// <summary>
    /// Статьи затрат
    /// </summary>
    public class CostItem : BaseGkhEntity
    {
        /// <summary>
        /// Базовая услуга
        /// </summary>
        public virtual BaseService BaseService { get; set; }

        /// <summary>
        /// Наименование статьи
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Количество
        /// </summary>
        public virtual decimal? Count { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        /// Цена за еденицу
        /// </summary>
        public virtual decimal? Cost { get; set; }
    }
}
