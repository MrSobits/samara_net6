namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Места проведения проверки
    /// </summary>
    public class AuditPlaceProxy : IHaveId
    {
        /// <inheritdoc />
        public long Id { get; set; }

        /// <summary>
        /// 1. Проверка
        /// </summary>
        public long AuditId { get; set; }

        /// <summary>
        /// 2. Порядковый номер
        /// </summary>
        public long Number => this.Id;

        /// <summary>
        /// 3. Дом
        /// </summary>
        [ProxyId(typeof(HouseProxy))]
        public long? HouseId { get; set; }

        /// <summary>
        /// 4. Дополнительная информация
        /// </summary>
        public string AdditionalInfo { get; set; }
    }
}