namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;

    /// <summary>
    /// Работы/услуги перечня
    /// </summary>
    public class WorkProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код работы/услуги
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Перечень работ/услуг на период
        /// </summary>
        public long? WorkUslugaProxyId { get; set; }

        /// <summary>
        /// 3. Цена
        /// </summary>
        public decimal? Cost { get; set; }

        /// <summary>
        /// 4. Объём
        /// </summary>
        public decimal? Volume { get; set; }

        /// <summary>
        /// 5. Количество
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// 6. Общая стоимость
        /// </summary>
        public decimal? Sum { get; set; }

        /// <summary>
        /// 7. Работа/услуга организации
        /// </summary>
        public long? DictUslugaProxyId { get; set; }
    }
}