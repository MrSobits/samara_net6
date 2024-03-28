namespace Bars.Gkh.Gis.Entities.Kp50
{
    using B4.DataAccess;
    using Gkh.Entities;

    /// <summary>
    /// Банк данных
    /// </summary>
    public class GisDataBank : BaseEntity
    {
        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Район
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Наименование банка
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Ключ банка
        /// </summary>
        public virtual string Key { get; set; }
    }
}