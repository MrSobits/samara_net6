namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Проверки
    /// </summary>
    public class AuditProxy : IHaveId
    {
        /// <inheritdoc />
        public long Id { get; set; }

        /// <summary>
        /// 2. Контролирующий орган
        /// </summary>
        [ProxyId(typeof(GjiProxy))]
        public long? ContragentFrguId { get; set; }

        /// <summary>
        /// 3. Уникальный идентификатор функции контролирующего органа
        /// </summary>
        [ProxyId(typeof(GjiProxy))]
        public long? FrguId { get; set; }

        /// <summary>
        /// 4. Вид проверки
        /// </summary>
        public int? CheckType { get; set; }

        /// <summary>
        /// 5. Статус проверки
        /// </summary>
        public int? CheckState { get; set; }

        /// <summary>
        /// 6. План проверок
        /// </summary>
        [ProxyId(typeof(AuditPlanProxy))]
        public long? AuditPlanId { get; set; }

        /// <summary>
        /// 7. Порядковый номер в плане
        /// </summary>
        public int? PlanNumber { get; set; }

        /// <summary>
        /// 8. Учётный номер проверки в едином реестре проверок
        /// </summary>
        public int? RegistrationNumber { get; set; }

        /// <summary>
        /// 9. Дата присвоения учётного номера проверки
        /// </summary>
        public DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// 10. Дата окончания последней проверки
        /// </summary>
        public DateTime? LastAuditDate { get; set; }

        /// <summary>
        /// 11. Проверка должна быть зарегистрирована в едином реестре проверок
        /// </summary>
        public int? MustRegistered { get; set; }

        /// <summary>
        /// 12. Основание регистрации проверки в едином реестре проверок
        /// </summary>
        public int? RegistrationReason { get; set; }

        /// <summary>
        /// 13. Вид осуществления контрольной деятельности
        /// </summary>
        public int? AuditKind { get; set; }

        /// <summary>
        /// 14. Форма проведения проверки
        /// </summary>
        public int? AuditForm { get; set; }

        /// <summary>
        /// 15. Номер распоряжения (приказа)
        /// </summary>
        public string DisposalNumber { get; set; }

        /// <summary>
        /// 16. Дата утверждения распоряжения (приказа)
        /// </summary>
        public DateTime? DisposalDate { get; set; }

        /// <summary>
        /// 17. ФИО и должностных лиц, уполномоченных на проведение проверки
        /// </summary>
        public string Inspectors { get; set; }

        /// <summary>
        /// 18. ФИО и должность экспертов, привлекаемых к проведению проверки
        /// </summary>
        public string Param18 { get; set; }

        /// <summary>
        /// 19. Тип внеплановой проверки
        /// </summary>
        public string Param19 { get; set; }

        /// <summary>
        /// 20. Субъект проверки – ЮЛ,ИП
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? SubjectContragentId { get; set; }

        /// <summary>
        /// 21. Субъект проверки – Гражданин
        /// </summary>
        [ProxyId(typeof(IndProxy))]
        public long? SubjectPhysicalId { get; set; }

        /// <summary>
        /// 22. Субъект проверки является субъектом малого предпринимательства
        /// </summary>
        public int? IsSubjectSmallBusiness { get; set; }

        /// <summary>
        /// 23. Место фактического осуществления деятельности
        /// </summary>
        public string FactActionPlace { get; set; }

        /// <summary>
        /// 24. Другие физ. Лица, в отношении которых проводится проверка
        /// </summary>
        public string Param24 { get; set; }

        /// <summary>
        /// 25. Статус уведомления
        /// </summary>
        public int? NotificationState { get; set; }

        /// <summary>
        /// 26. Дата уведомления
        /// </summary>
        public DateTime? NotificationDate { get; set; }

        /// <summary>
        /// 27. Способ уведомления о проведении проверки
        /// </summary>
        public string NotificationMethod { get; set; }

        /// <summary>
        /// 28. Основание проведения проверки
        /// </summary>
        public int? AuditReason { get; set; }

        /// <summary>
        /// 29. Предписание, на основании которого проводится проверка
        /// </summary>
        public string Param29 { get; set; }

        /// <summary>
        /// 30. Связанная проверка
        /// </summary>
        public long? DependentAuditId { get; set; }

        /// <summary>
        /// 31. Цель проведения проверки с реквизитами документов основания
        /// </summary>
        public string AuditReasonPurpose { get; set; }

        /// <summary>
        /// 32. Дополнительная информация об основаниях проведения проверки
        /// </summary>
        public string Param32 { get; set; }

        /// <summary>
        /// 33. Задачи проведения проверки
        /// </summary>
        public string AuditTask { get; set; }

        /// <summary>
        /// 34. Дата начала проведения проверки
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 35. Дата окончания проведения проверки
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 36. Срок проведения проверки-Рабочих дней
        /// </summary>
        public int? WorkDaysCount { get; set; }

        /// <summary>
        /// 37. Срок проведения проверки-Рабочих часов
        /// </summary>
        public int? WorkHoursCount { get; set; }

        /// <summary>
        /// 38. Орган государственного надзора (контроля) и/или орган муниципального контроля, с которым проверка проводится совместно
        /// </summary>
        public string AuditCompanion { get; set; }

        /// <summary>
        /// 39. Наличие информации о согласовании проверки с органами прокуратуры.
        /// </summary>
        public int? ProsecutorAgreed { get; set; }

        /// <summary>
        /// 40. Проверка согласована
        /// </summary>
        public int? IsAgreed { get; set; }

        /// <summary>
        /// 41. Номер приказа о согласовании(отказе) в проведении проверки
        /// </summary>
        public string AgreedorderNumber { get; set; }

        /// <summary>
        /// 42. Дата приказа о согласовании(отказе) в проведении проверки
        /// </summary>
        public DateTime? AgreedorderDate { get; set; }

        /// <summary>
        /// 43. Дата вынесения решения о согласовании (отказе) в проведении проверки
        /// </summary>
        public string Param43 { get; set; }

        /// <summary>
        /// 44. Место вынесения решения о согласовании (отказе) в проведении проверки
        /// </summary>
        public string Param44 { get; set; }

        /// <summary>
        /// 45. Должность подписанта
        /// </summary>
        public string Param45 { get; set; }

        /// <summary>
        /// 46. ФИО подписанта
        /// </summary>
        public string Param46 { get; set; }

        /// <summary>
        /// 47. Дополнительная информация о проверке
        /// </summary>
        public string Param47 { get; set; }

        /// <summary>
        /// 48. Причины невозможности проведения проверки
        /// </summary>
        public string Param48 { get; set; }

        /// <summary>
        /// 49. Причина изменения проверки
        /// </summary>
        public string Param49 { get; set; }

        /// <summary>
        /// 50. Дата изменения проверки
        /// </summary>
        public string Param50 { get; set; }

        /// <summary>
        /// 51. Номер решения об изменении проверки
        /// </summary>
        public string Param51 { get; set; }

        /// <summary>
        /// 52. Дополнительная информация об изменения проверки
        /// </summary>
        public string Param52 { get; set; }

        /// <summary>
        /// 53. Организация, принявшая решение об изменении проверки
        /// </summary>
        public string Param53 { get; set; }

        /// <summary>
        /// 54. Причина отмены проверки
        /// </summary>
        public string Param54 { get; set; }

        /// <summary>
        /// 55. Дата отмены проверки
        /// </summary>
        public string Param55 { get; set; }

        /// <summary>
        /// 56. Номер решения об отмене проверки
        /// </summary>
        public string Param56 { get; set; }

        /// <summary>
        /// 57. Дополнительная информация об отмене проверки
        /// </summary>
        public string Param57 { get; set; }

        /// <summary>
        /// 58. Организация, принявшая решение об отмене проверки
        /// </summary>
        public string Param58 { get; set; }

        /// <summary>
        /// Идентификатор проверки
        /// </summary>
        public long InspectionId { get; set; }

        /// <summary>
        /// Идентификатор распоряжения
        /// </summary>
        public long DisposalId { get; set; }

        /// <summary>
        /// Идентификатор акта проверки
        /// </summary>
        public long ActCheckId { get; set; }
    }
}