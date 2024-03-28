namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// View документов постановлений Роспотребнадзора с количественными и агрегированными показателями
    /// </summary>
    /// <remarks>
    /// сумма оплат штрафа,
    /// строка идентификаторов жилых домов(из родительского исполнительного документа) вида /1/2/4/ для фильтрации в реестре документов
    /// </remarks>
    public class ViewResolutionRospotrebnadzor : PersistentObject
    {
        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Постановление
        /// </summary>
        public virtual long? ResolutionId { get; set; }

        /// <summary>
        /// Сумма оплат штрафов
        /// </summary>
        public virtual decimal? SumPays { get; set; }

        /// <summary>
        /// строка идентификаторов жилых домов вида /1/2/4/
        /// </summary>
        public virtual string RealityObjectIds { get; set; }

        /// <summary>
        /// Наименования муниципальных образований жилых домов
        /// </summary>
        public virtual string MunicipalityNames { get; set; }

        /// <summary>
        /// Наименования муниципальных образований жилых домов
        /// </summary>
        public virtual string MoNames { get; set; }

        /// <summary>
        /// Наименования населенных пунктов жилых домов
        /// </summary>
        public virtual string PlaceNames { get; set; }

        /// <summary>
        /// Муниципальное образование первого жилого дома
        /// </summary>
        public virtual long? MunicipalityId { get; set; }

        /// <summary>
        /// ФИО ДЛ вынесшего постановление
        /// </summary>
        public virtual string OfficialName { get; set; }

        /// <summary>
        /// Идентификатор ДЛ вынесшего постановление
        /// </summary>
        public virtual long? OfficialId { get; set; }

        /// <summary>
        /// Сумма штрафа
        /// </summary>
        public virtual decimal? PenaltyAmount { get; set; }

        /// <summary>
        /// Идентификатор основания проверки
        /// </summary>
        public virtual long? InspectionId { get; set; }

        /// <summary>
        /// Тип основания проверки
        /// </summary>
        public virtual TypeBase TypeBase { get; set; }

        /// <summary>
        /// Тип исполнителя
        /// </summary>
        public virtual string TypeExecutant { get; set; }

        /// <summary>
        /// Санкция
        /// </summary>
        public virtual string Sanction { get; set; }

        /// <summary>
        /// Контрагент МО Id
        /// </summary>
        public virtual long? ContragentMuId { get; set; }

        /// <summary>
        /// Контрагент МО Name
        /// </summary>
        public virtual string ContragentMuName { get; set; }

        /// <summary>
        /// Контрагент (исполнитель)
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Целая часть номера документа
        /// </summary>
        public virtual int? DocumentNum { get; set; }

        /// <summary>
        /// Тип документа ГЖИ
        /// </summary>
        public virtual TypeDocumentGji TypeDocumentGji { get; set; }

        /// <summary>
        /// Дата вручения
        /// </summary>
        public virtual DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Штраф оплачен
        /// </summary>
        public virtual YesNo Paided { get; set; }

        /// <summary>
        /// Адреса по нарушениям
        /// </summary>
        public virtual string RoAddress { get; set; }
    }
}