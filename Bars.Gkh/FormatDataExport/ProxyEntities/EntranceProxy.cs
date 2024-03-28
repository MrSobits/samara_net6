namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Подъезд
    /// </summary>
    public class EntranceProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код помещения в системе отправителя
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Уникальный идентификатор дома
        /// </summary>
        [ProxyId(typeof(HouseProxy))]
        public long? HouseId { get; set; }

        /// <summary>
        /// 3. Номер подъезда
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 4. Этажность
        /// </summary>
        public int? FloorSize { get; set; }

        /// <summary>
        /// 5. Дата постройки
        /// </summary>
        public DateTime? BuildDate { get; set; }

        /// <summary>
        /// 6. Дата прекращения существования объекта
        /// </summary>
        public DateTime? TerminationDate { get; set; }

        /// <summary>
        /// 7. Информация подтверждена поставщиком, ответственным за размещение сведений
        /// </summary>
        public int? IsSupplierConfirmed { get; set; }
    }
}
