namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Кредитные договоры/договоры займа (creditcontract.csv)
    /// </summary>
    public class CreditContractProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный идентификатор договора
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Тип договора
        /// </summary>
        public int? Type => 2;

        /// <summary>
        /// 3. Кредитор / Займодатель
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? Creditor { get; set; }

        /// <summary>
        /// 4. Номер договора
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 5. Дата договора
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// 6. Договор бессрочный
        /// </summary>
        public int? IsUnlimited => 1;

        /// <summary>
        /// 7. Срок договора в месяцах
        /// </summary>
        public int? CountMounthPeriod { get; set; }

        /// <summary>
        /// 8. Договор беспроцентный
        /// </summary>
        public int? IsNoPercent => 1;

        /// <summary>
        /// 9. Процентная ставка
        /// </summary>
        public decimal? PercentRate { get; set; }

        /// <summary>
        /// 10. Примечание к процентной ставке
        /// </summary>
        public string NotationToPercentRate { get; set; }

        /// <summary>
        /// 11. Размер договора
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 12. Примечание к размеру договора
        /// </summary>
        public string NotationToSize { get; set; }

        /// <summary>
        /// 13. Статус договора
        /// </summary>
        public int? State => 1;

        /// <summary>
        /// 14. Причина
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 15. Номер документа
        /// </summary>
        public string DocNumber { get; set; }

        /// <summary>
        /// 16. Дата документа
        /// </summary>
        public DateTime? DocDate { get; set; }

        /// <summary>
        /// CREDITCONTRACTFILES 1. Идентификатор файла
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        /// CREDITCONTRACTFILES 3. Тип файла
        /// </summary>
        public int FileType { get; set; }
    }
}