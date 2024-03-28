namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Работы/услуги по договору управления
    /// </summary>
    public class DuServiceProxy : IHaveId
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