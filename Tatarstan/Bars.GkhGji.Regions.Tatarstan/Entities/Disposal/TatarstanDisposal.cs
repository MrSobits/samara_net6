namespace Bars.GkhGji.Regions.Tatarstan.Entities
{
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Enums;
    using System;
    using ControlType = GkhGji.Entities.Dict.ControlType;

    /// <summary>
    /// Рапоряжение ГЖИ для Татарстана (расширяется дополнительными полями)
    /// </summary>
    /// <remarks>
    /// Этот документ является базовым для документа "Решение"
    /// Метод GetAll() домен-сервиса возвращает только "Распоряжения"
    /// ОГРАНИЧЕНИЕ - накладывается where, поэтому доступен только IEnumerable Join
    /// 
    /// Использование IRepository для TatarstanDisposal говорит о том, что
    /// участок кода подразумевает работу с обоими документами или
    /// обход ограничения метода GetAll() домен-сервиса 
    /// </remarks>
    public class TatarstanDisposal : Bars.GkhGji.Entities.Disposal
    {
        /// <summary>
        /// Учетный номер проверки в ЕРП
        /// </summary>
        public virtual string ErpRegistrationNumber { get; set; }

        /// <summary>
        /// Идентификатор ЕРП
        /// </summary>
        public virtual string ErpId { get; set; }

        /// <summary>
        /// Дата присвоения учетного номера / идентификатора ЕРП
        /// </summary>
        public virtual DateTime? ErpRegistrationDate { get; set; }

        /// <summary>
        /// Срок проверки (количество дней)
        /// </summary>
        public virtual int? CountDays { get; set; }

        /// <summary>
        /// Срок проверки (количество часов)
        /// </summary>
        public virtual int? CountHours { get; set; }

        /// <summary>
        /// Наименование прокуратуры
        /// </summary>
        public virtual ProsecutorOfficeDict Prosecutor { get; set; }

        /// <summary>
        /// Основание для включения проверки в ЕРП
        /// </summary>
        public virtual ReasonErpChecking? ReasonErpChecking { get; set; }

        /// <summary>
        /// Основание проверки
        /// </summary>
        public virtual InspectionBaseType InspectionBase { get; set; }

        /// <summary>
        /// Способ уведомления
        /// </summary>
        public virtual NotificationType? NotificationType { get; set; }

        /// <summary>
        /// Параметр, показывающий было ли распоряжение отправлено в ЕРП
        /// </summary>
        public virtual bool? IsSentToErp { get; set; }

        /// <summary>
        /// Вид контроля
        /// </summary>
        public virtual ControlType ControlType { get; set; }

        /// <summary>
        /// Идентификатор ТОР карточки проверки
        /// </summary>
        public virtual Guid? ControlCardId { get; set; }

        /// <summary>
        /// Идентификатор ТОР Результата проверки
        /// </summary>
        public virtual Guid? ResultTorId { get; set; }

        /// <summary>
        /// Время документа
        /// </summary>
        public virtual DateTime? DocumentTime { get; set; }

        /// <summary>
        /// Срок взаимодействия с контролируемым лицом не более, часов
        /// </summary>
        public virtual int? InteractionPersonHour { get; set; }

        /// <summary>
        /// Срок взаимодействия с контролируемым лицом не более, минут
        /// </summary>
        public virtual int? InteractionPersonMinutes { get; set; }

        /// <summary>
        /// Основание для приостановления проведения проверки
        /// </summary>
        public virtual string SuspensionInspectionBase { get; set; }

        /// <summary>
        /// Дата начала периода приостановления проведения проверки
        /// </summary>
        public virtual DateTime? SuspensionDateFrom { get; set; }

        /// <summary>
        /// Дата окончания периода приостановления проведения проверки
        /// </summary>
        public virtual DateTime? SuspensionDateTo { get; set; }

        /// <summary>
        /// Время начала периода приостановления проведения проверки
        /// </summary>
        public virtual DateTime? SuspensionTimeFrom { get; set; }

        /// <summary>
        /// Время окончания периода приостановления проведения проверки
        /// </summary>
        public virtual DateTime? SuspensionTimeTo { get; set; }

        /// <summary>
        /// Сведения о причинении вреда (ущерба) (ст. 66 ФЗ)
        /// </summary>
        public virtual string InformationAboutHarm { get; set; }
    }
}