namespace Bars.GkhGji.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;

    /*
     * Данная вьюха предназначена для реестра Актов проверки предписаний 
     * с количественными и агрегированными показателями:
     * ФИО инспекторов, 
     * количество жилых домов по которым составлен акт, 
     * количество исполнительных документов, созданных из акта,
     * идентификаторы жилых домов в строке вида /1/2/4/ для фильтрации в реестре документов, 
     * юридическое лицо первого родительского исполнительного документа
     * первый родительский документ и его наименование
     * количество родительских исполнительных документов
     */
    public class ViewActRemoval : PersistentObject
    {
        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Акт проверки предписания (Он же акт устранения нарушений)
        /// </summary>
        public virtual long? ActRemovalGjiId { get; set; }

        /// <summary>
        /// Инспекторы
        /// </summary>
        public virtual string InspectorNames { get; set; }

        /// <summary>
        /// Количество домов
        /// </summary>
        public virtual int? RealityObjectCount { get; set; }

        /// <summary>
        /// Юридическое лицо - то есть родительского документа (Предписание/Протокол)
        /// </summary>
        public virtual string ParentContragent { get; set; }

        /// <summary>
        /// Контрагент МО Id 
        /// </summary>
        public virtual long? ParentContragentMuId { get; set; }

        /// <summary>
        /// Контрагент МО Name
        /// </summary>
        public virtual string ParentContragentMuName { get; set; }

        /// <summary>
        /// Документ ГЖИ  тоесть родительского документа
        /// </summary>
        public virtual long? ParentDocumentId { get; set; }

        /// <summary>
        /// Наименование родительского документа
        /// </summary>
        public virtual string ParentDocumentName { get; set; }

        /// <summary>
        /// Количество исполнительных документов
        /// </summary>
        public virtual int? CountExecDoc { get; set; }

        /// <summary>
        /// строка идентификаторов жилых домов вида /1/2/4/ 
        /// </summary>
        public virtual string RealityObjectIds { get; set; }
        
        /// <summary>
        /// Адреса жилых домов
        /// </summary>
        /// <remarks>Из этапов нарушений (разделитель - ';')</remarks>
        public virtual string RealityObjectAddresses { get; set; }

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
        /// Мунниципальное образование первого жилого дома
        /// </summary>
        public virtual long? MunicipalityId { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Целая часть номера документа
        /// </summary>
        public virtual int? DocumentNum { get; set; }

        /// <summary>
        /// Тип родительского документа
        /// </summary>
        public virtual TypeDocument TypeParentDocument { get; set; }

        /// <summary>
        /// Идентификатор основания проверки
        /// </summary>
        public virtual long? InspectionId { get; set; }

        /// <summary>
        /// Тип основания проверки
        /// </summary>
        public virtual TypeBase TypeBase { get; set; }

        /// <summary>
        /// Признак устранено нарушение или нет
        /// </summary>
        public virtual YesNoNotSet TypeRemoval { get; set; }

        /// <summary>
        /// Тип документа ГЖИ
        /// </summary>
        public virtual TypeDocumentGji TypeDocumentGji { get; set; }

        /// <summary>
        /// Вид контроля(надзора)
        /// </summary>
        public virtual ControlType ControlType { get; set; }
    }
}