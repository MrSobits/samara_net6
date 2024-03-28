namespace Bars.GkhCr.Entities
{
    using Gkh.Entities;

    /// <summary>
    /// Квалификационный отбор
    /// </summary>
    public class Qualification : BaseGkhEntity
    {
        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual ObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Подрядчик
        /// </summary>
        public virtual Builder Builder { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal? Sum { get; set; }
    }
}