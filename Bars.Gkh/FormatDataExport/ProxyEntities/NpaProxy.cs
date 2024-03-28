namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Нормативные правовые акты
    /// </summary>
    public class NpaProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Уровень органа власти
        /// </summary>
        public int? AuthLevel { get; set; }

        /// <summary>
        /// 3. Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 4. Дата принятия документа
        /// </summary>
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// 5. Номер
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 6. Наименование принявшего акт органа
        /// </summary>
        public string AuthName { get; set; }

        /// <summary>
        /// 7. Тип информации в НПА
        /// </summary>
        public string InfoType { get; set; }

        /// <summary>
        /// 8. Тип НПА
        /// </summary>
        public string ActType { get; set; }

        /// <summary>
        /// 9. Вид нормативного акта
        /// </summary>
        public string ActKind { get; set; }

        /// <summary>
        /// 10. Орган принявший НПА
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? ContragentId { get; set; }

        /// <summary>
        /// 11. Дата вступления в силу
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 12. Дата утраты силы
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 13. Субъект РФ
        /// </summary>
        public long? Subject { get; set; }

        /// <summary>
        /// 14. Документ действует на всей территории субъекта РФ
        /// </summary>
        public int? IsThroughoutTerritoryValid { get; set; }

        /// <summary>
        /// 15. Файл вложение
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        /// 16. Статус записи
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 17. Причина аннулирования
        /// </summary>
        public string TerminationReason { get; set; }
    }
}