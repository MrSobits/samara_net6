namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Документы об утверждении тарифов ЖКУ
    /// </summary>
    public class TarifProxy : IHaveId
    {
        /// <inheritdoc />
        public long Id { get; set; }

        /// <summary>
        /// 2. Наименование документа
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// 3. Номер документа
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// 4. Дата принятия документа органом власти
        /// </summary>
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// 5. Дата начала действия тарифа
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 6. Дата окончания действия тарифа
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 7. Опубликован
        /// </summary>
        public int IsPublished { get; set; } = 1;

        /// <summary>
        /// 8. Код региона ФИАС
        /// </summary>
        public int RegionCode { get; set; }

        /// <summary>
        /// 9. Вид тарифа
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 10. Скан-копия документа
        /// </summary>
        public FileInfo AttachmentFile { get; set; }

        /// <summary>
        /// TARIFRSO 2. РСО
        /// </summary>
        public long? RsoContragentId { get; set; }

        /// <summary>
        /// TARIFOKTMO 2. ОКТМО
        /// </summary>
        public long? Oktmo { get; set; }

        /// <summary>
        /// TARIFUSLUGA 2. Вид коммунальной услуги
        /// </summary>
        public int? CommunalServiceType { get; set; }

        /// <summary>
        /// TARIFUSLUGA 3. Тариф
        /// </summary>
        public decimal? TarifCost { get; set; }

        /// <summary>
        /// TARIFUSLUGA 4. Количество компонентов в цене тарифа
        /// </summary>
        public int? TarifComponentCount { get; set; }

        /// <summary>
        /// TARIFUSLUGA 5. Количество ставок тарифа
        /// </summary>
        public int? TarifRateCount { get; set; }

        /// <summary>
        /// TARIFRES 2. Вид коммунального ресурса
        /// </summary>
        public int? CommunalResourceType { get; set; }
    }
}