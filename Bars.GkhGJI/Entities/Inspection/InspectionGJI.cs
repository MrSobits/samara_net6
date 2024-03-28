namespace Bars.GkhGji.Entities
{
    using System;

    using B4.Modules.States;

    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.Entities.Dict;

    using Gkh.Enums;
    using Gkh.Entities;
    using Enums;

    using ControlType = Bars.GkhGji.Enums.ControlType;

    /// <summary>
    /// Проверка ГЖИ
    /// </summary>
    public class InspectionGji : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Контрагент проверки
        /// </summary>
        public virtual Contragent Contragent { get; set; }
        
        /// <summary>
        /// Категория риска
        /// </summary>
        /// <remarks>Не хранимое поле</remarks>
        public virtual Bars.Gkh.Entities.Dicts.RiskCategory RiskCategory { get; set; }

        /// <summary>
        /// Тип юридического лица
        /// </summary>
        public virtual TypeJurPerson TypeJurPerson { get; set; }

        /// <summary>
        /// Тип основания проверки
        /// </summary>
        public virtual TypeBase TypeBase { get; set; }

        /// <summary>
        /// Основание проверки
        /// </summary>
        public virtual InspectionBaseType InspectionBaseType { get; set; }

        //ToDo ГЖИ выпилить все нехранимые поля 

        /// <summary>
        /// Не хранимое поле. идентификатор распоряжения. Для того чтобы несколько раз нельзя было делать распоряжения
        /// </summary>
        public virtual long? DisposalId { get; set; }
        
        /// <summary>
        /// Дата начала категории риска
        /// </summary>
        /// <remarks>Не хранимое поле</remarks>
        public virtual DateTime? RiskCategoryStartDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string InspectionNumber { get; set; }

        /// <summary>
        /// Номер документа (Целая часть)
        /// </summary>
        public virtual int? InspectionNum { get; set; }

        /// <summary>
        /// Год документа
        /// </summary>
        public virtual int? InspectionYear { get; set; }

        // Если проверка без основания то при конвертации нужно было сохранять Физ лицо если это требовалось
        /// <summary>
        /// Физическое лицо
        /// </summary>
        public virtual string PhysicalPerson { get; set; }

        /// <summary>
        /// Реквизиты физ. лица
        /// </summary>
        public virtual string PhysicalPersonInfo { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Объект проверки
        /// </summary>
        public virtual PersonInspection PersonInspection { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Учетный номер проверки в едином реестре проверок
        /// </summary>
        public virtual string RegistrationNumber { get; set; }

        /// <summary>
        /// Дата присвоения учетного номера
        /// </summary>
        public virtual DateTime? RegistrationNumberDate { get; set; }

        /// <summary>
        /// Срок проверки (количество дней)
        /// </summary>
        public virtual int? CheckDayCount { get; set; }

        /// <summary>
        /// Дата проверки
        /// </summary>
        public virtual DateTime? CheckDate { get; set; }

        /// <summary>
        /// Вид контроля
        /// </summary>
        public virtual ControlType? ControlType { get; set; }

        /// <summary>
        /// Основание для включения проверки в ЕРП
        /// </summary>
        public virtual ReasonErpChecking ReasonErpChecking { get; set; }

        /// <summary>
        /// Вид контроля(надзора)
        /// </summary>
        public virtual KindKNDGJI KindKNDGJI { get; set; }
    }
}