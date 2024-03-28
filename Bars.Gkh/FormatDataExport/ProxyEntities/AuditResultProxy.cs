namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Результаты проверок
    /// </summary>
    public class AuditResultProxy : IHaveId
    {
        /// <inheritdoc />
        public long Id { get; set; }

        /// <summary>
        /// 1. Уникальный код проверки
        /// </summary>
        [ProxyId(typeof(AuditProxy))]
        public long AuditId { get; set; }

        /// <summary>
        /// 2. Статус результата проверки
        /// </summary>
        public int? State { get; set; }

        /// <summary>
        /// 3. Вид документа результата проверки
        /// </summary>
        public int? DocumentKind { get; set; }

        /// <summary>
        /// 4. Номер документа результата проверки
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// 5. Дата составления документа результата проверки
        /// </summary>
        public DateTime? DocumentDate{ get; set; }

        /// <summary>
        /// 6. Результат проверки
        /// </summary>
        public int? Result { get; set; }

        /// <summary>
        /// 7. Характер нарушения
        /// </summary>
        public string Violations { get; set; }

        /// <summary>
        /// 8. Несоответствие поданных сведений о начале осуществления предпринимательской деятельности
        /// </summary>
        public string Param8 { get; set; }

        /// <summary>
        /// 9. Положение нарушенного правового акта
        /// </summary>
        public string ActViolations { get; set; }

        /// <summary>
        /// 10. Другие несоответствия поданных сведений
        /// </summary>
        public string Param10 { get; set; }

        /// <summary>
        /// 11. Список лиц допустивших нарушение
        /// </summary>
        public string Param11 { get; set; }

        /// <summary>
        /// 12. Орган, в который отправлены материалы о выявленных нарушениях
        /// </summary>
        public string Param12 { get; set; }

        /// <summary>
        /// 13. Дата отправления материалов в ОГВ
        /// </summary>
        public string Param13 { get; set; }

        /// <summary>
        /// 14. Перечень применённых мер обеспечения производства по делу об административном правонарушении
        /// </summary>
        public string Param14 { get; set; }

        /// <summary>
        /// 15. Информация о привлечении проверяемых лиц к административной ответственности
        /// </summary>
        public string Param15 { get; set; }

        /// <summary>
        /// 16. Информация об аннулировании или приостановлении документов, имеющих разрешительный характер
        /// </summary>
        public string Param16 { get; set; }

        /// <summary>
        /// 17. Информация об обжаловании решений органа контроля
        /// </summary>
        public string Param17 { get; set; }

        /// <summary>
        /// 18. Дата и время начала проведения проверки
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 19. Дата окончания проведения проверки
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 20. Продолжительность проведения проверки (дней)
        /// </summary>
        public int? DayCount { get; set; }

        /// <summary>
        /// 21. Продолжительность проведения проверки (часов)
        /// </summary>
        public int? HourCount { get; set; }

        /// <summary>
        /// 22. Место проведения проверки
        /// </summary>
        public string PlaceAddress { get; set; }

        /// <summary>
        /// 23. ФИО должностных лиц, проводивших проверку
        /// </summary>
        public string Inspectors { get; set; }

        /// <summary>
        /// 24. ФИО и должность представителей субъекта проверки, присутствующих при проведении проверки
        /// </summary>
        public string Respondents { get; set; }

        /// <summary>
        /// 25. Место составления документа результата проверки
        /// </summary>
        public string CreateDocPlaceAddress { get; set; }

        /// <summary>
        /// 26. Дополнительная информация о результате проверки
        /// </summary>
        public string Param26 { get; set; }

        /// <summary>
        /// 27. Статус ознакомления с результатом проверки
        /// </summary>
        public int? ReviewState { get; set; }

        /// <summary>
        /// 28. ФИО должностного лица, отказавшегося от ознакомления с актом проверки
        /// </summary>
        public string RefusedRespondents { get; set; }

        /// <summary>
        /// 29. Дата ознакомления
        /// </summary>
        public DateTime? ReiviewDate { get; set; }

        /// <summary>
        /// 30. Наличие подписи
        /// </summary>
        public int? SignExists { get; set; }

        /// <summary>
        /// 31. ФИО должностного лица, ознакомившегося с актом проверки
        /// </summary>
        public string ConcentRespondents { get; set; }

        /// <summary>
        /// 32. Причина отмены
        /// </summary>
        public string Param32 { get; set; }

        /// <summary>
        /// 33. Дата отмены
        /// </summary>
        public string Param33 { get; set; }

        /// <summary>
        /// 34. Номер решения об отмене
        /// </summary>
        public string Param34 { get; set; }

        /// <summary>
        /// 35. Организация, принявшая решение об отмене
        /// </summary>
        public string Param35 { get; set; }

        /// <summary>
        /// 36. Дополнительная информация об отмене
        /// </summary>
        public string Param36 { get; set; }
    }
}