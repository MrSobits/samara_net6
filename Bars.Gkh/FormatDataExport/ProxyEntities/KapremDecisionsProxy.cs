namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Решения по капитальному ремонту (kapremdecisions.csv)
    /// </summary>
    public class KapremDecisionsProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный идентификатор протокола
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Адрес дома
        /// </summary>
        [ProxyId(typeof(HouseProxy))]
        public long? HouseId { get; set; }

        /// <summary>
        /// 3. Статус
        /// <para>По умолчанию статус у всех "Не активен".
        /// Статус "Активен" проставляется при формировании секции
        /// у самого позднего протокола по дому
        /// </para>
        /// </summary>
        public int? State { get; set; } = 2;

        /// <summary>
        /// 4. Основание принятия решения
        /// </summary>
        public int? BasisReason { get; set; }

        /// <summary>
        /// 5. Тип решения
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 6. Дата вступления в силу
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 7. Способ формирования фонда
        /// </summary>
        public int? FundType { get; set; }

        /// <summary>
        /// 8. Размер превышения взноса на КР
        /// </summary>
        public decimal? CrPayment { get; set; }

        /// <summary>
        /// 9. Протокол ОСС
        /// </summary>
        [ProxyId(typeof(ProtocolossProxy))]
        public long? ProtocolId { get; set; }

        /// <summary>
        /// 10. Номер протокола
        /// </summary>
        public string ProtocolNumber { get; set; }

        /// <summary>
        /// 11. Дата протокола
        /// </summary>
        public DateTime? ProtocolDate { get; set; }

        /// <summary>
        /// 12. Наименование документа решения
        /// </summary>
        public string DocName { get; set; }

        /// <summary>
        /// 13. Вид документа
        /// </summary>
        public string DocKind { get; set; }

        /// <summary>
        /// 14. Номер документа
        /// </summary>
        public string DocNumber { get; set; }

        /// <summary>
        /// 15. Дата документа
        /// </summary>
        public DateTime? DocDate { get; set; }

        /// <summary>
        /// 16. НПА
        /// </summary>
        [ProxyId(typeof(NpaProxy))]
        public long? NpaId { get; set; }

        /// <summary>
        /// 17. Расчетный счет
        /// </summary>
        [ProxyId(typeof(ContragentRschetProxy))]
        public long? ContragentRschetId { get; set; }

        /// <summary>
        /// 17. Расчетный счет (Регоп)
        /// </summary>
        [ProxyId(typeof(RegopSchetProxy))]
        public long? RegopSchetId { get; set; }

        /// <summary>
        /// 1. Уникальный идентификатор файла
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        /// 3. Тип файла
        /// </summary>
        public int? FileType { get; set; }
    }
}