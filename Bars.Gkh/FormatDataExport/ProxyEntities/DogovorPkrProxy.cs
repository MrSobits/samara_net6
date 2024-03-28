namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Договоры на выполнение работ по капитальному ремонту
    /// </summary>
    public class DogovorPkrProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Идентификатор договора подряда КР
        /// </summary>
        public long? BuildContractId { get; set; }

        /// <summary>
        /// 2. Идентификатор ПКР
        /// </summary>
        [ProxyId(typeof(PkrProxy))]
        public long? PkrId { get; set; }

        /// <summary>
        /// 3. Номер договора
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// 4. Дата договора
        /// </summary>
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// 5. Дата начала выполнения
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 6. Дата окончания выполнения работ
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 7. Сумма договора
        /// </summary>
        public decimal? Sum { get; set; }

        /// <summary>
        /// 8. Заказчиком является
        /// </summary>
        public int? IsCustomer { get; set; }

        /// <summary>
        /// 9. Фамилия заказчика
        /// </summary>
        public string CustomerSurname { get; set; }

        /// <summary>
        /// 10. Имя заказчика
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 11. Отчество заказчика
        /// </summary>
        public string CustomerPatronymic { get; set; }

        /// <summary>
        /// 12. Заказчик - организация
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? CustomerContragentId { get; set; }

        /// <summary>
        /// 13. Исполнитель
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? ExecutantContragentId { get; set; }

        /// <summary>
        /// 14. Гарантийный срок установлен
        /// </summary>
        public int IsGuaranteePeriod { get; set; }

        /// <summary>
        /// 15. Гарантийный срок (кол-во месяцев)
        /// </summary>
        public int? GuaranteePeriod { get; set; }

        /// <summary>
        /// 16. Наличие сметной документации
        /// </summary>
        public int? IsBudgetDocumentation { get; set; }

        /// <summary>
        /// 17. Проведение отбора предусмотрено законодательством
        /// </summary>
        public int? IsLawProvided { get; set; }

        /// <summary>
        /// 18. Адрес сайта с информацией об отборе
        /// </summary>
        public string InfoUrl { get; set; }

        /// <summary>
        /// 19. Статус договора
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 20. Причина расторжения
        /// </summary>
        public long? RevocationReason { get; set; }

        /// <summary>
        /// 21. Номер документа расторжения
        /// </summary>
        public string RevocationDocumentNumber { get; set; }

        /// <summary>
        /// 22. Дата документа расторжения
        /// </summary>
        public DateTime? RevocationDate { get; set; }

        #region DOGOVORPKRFILES
        /// <summary>
        /// DOGOVORPKRFILES 1. Уникальный идентификатор файла
        /// </summary>
        public FileInfo File { get; set; }
        
        /// <summary>
        /// DOGOVORPKRFILES 3. Тип файла
        /// </summary>
        public int FileType { get; set; }
        #endregion

        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public long? ObjectCrId { get; set; }
    }
}