namespace Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction
{
    using System;

    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Профилактическое мероприятие
    /// </summary>
    public class PreventiveAction : DocumentGji
    {
        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Вид мероприятия
        /// </summary>
        public virtual PreventiveActionType ActionType { get; set; }

        /// <summary>
        /// Тип визита
        /// </summary>
        public virtual PreventiveActionVisitType? VisitType { get; set; }

        /// <summary>
        /// Контролируемое лицо
        /// </summary>
        public virtual Contragent ControlledOrganization { get; set; }

        /// <summary>
        /// План
        /// </summary>
        public virtual PlanActionGji Plan { get; set; }

        /// <summary>
        /// Руководитель
        /// </summary>
        public virtual Inspector Head { get; set; }
        
        /// <summary>
        /// Тип подконтрольного лица
        /// </summary>
        public virtual TypeJurPerson? ControlledPersonType { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string FullName { get; set; }
        
        /// <summary>
        /// Телефон
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// Адрес нахождения контролируемого лица
        /// </summary>
        public virtual FiasAddress ControlledPersonAddress { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Наименование файла
        /// </summary>
        public virtual string FileName { get; set; }
        
        /// <summary>
        /// Номер файла
        /// </summary>
        public virtual string FileNumber { get; set; }
        
        /// <summary>
        /// Дата файла
        /// </summary>
        public virtual DateTime? FileDate { get; set; }

        /// <summary>
        /// Орган ГЖИ
        /// </summary>
        public virtual ZonalInspection ZonalInspection { get; set; }

        /// <summary>
        /// Вид контроля
        /// </summary>
        public virtual ControlType ControlType { get; set; }

        /// <summary>
        /// Учетный номер ПМ в ЕРКНМ
        /// </summary>
        public virtual string ErknmRegistrationNumber { get; set; }

        /// <summary>
        /// Дата присвоения учетного номера / идентификатора ЕРКНМ
        /// </summary>
        public virtual DateTime? ErknmRegistrationDate { get; set; }
    }
}