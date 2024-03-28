namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;

    /// <summary>
    /// Перечень работ/услуг на период (worklist.csv)
    /// </summary>
    public class WorkListProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код работы/услуги
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Дом
        /// </summary>
        public long? RealityObjectId { get; set; }

        /// <summary>
        /// 3. Договор управления
        /// </summary>
        public long? ManOrgContractId { get; set; }

        /// <summary>
        /// 4. Период «С»
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 5. Период «По»
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 6. Статус
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Work 3. Цена
        /// </summary>
        public decimal? Cost { get; set; }

        /// <summary>
        /// Work 4. Объём
        /// </summary>
        public decimal? Volume { get; set; }

        /// <summary>
        /// Work 5. Количество
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// Work 6. Общая стоимость
        /// </summary>
        public decimal? Summary { get; set; }

        /// <summary>
        /// Work 7. Работа/услуга организации
        /// </summary>
        public long? DictUslugaId { get; set; }
    }
}