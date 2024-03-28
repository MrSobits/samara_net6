namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Работы/услуги по уставу (ustavservice.csv)
    /// </summary>
    public class UstavServiceProxy : IHaveId
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 1. Уникальный код записи сведений о размещении размера платы
        /// </summary>
        public long? DuId { get; set; }

        /// <summary>
        /// 2. Работа, услуга организации
        /// </summary>
        [ProxyId(typeof(DictUslugaProxy))]
        public long? ServiceId { get; set; }

        /// <summary>
        /// 3. Размер платы (цены, тарифа) за работы(услуги)
        /// </summary>
        public decimal? PaymentAmount { get; set; }
    }
}